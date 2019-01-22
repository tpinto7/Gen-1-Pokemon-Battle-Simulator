using System;
using static System.Console;
using System.Collections;
using System.Collections.Generic;
using PokemonBattle;
using PokemonTypes;

namespace PokemonMoves
{
	abstract class Moves
	{
		public Types Type{ get; set; }
		public string Category{ get; set; }
		public int Priority{ get; set; }
		public int CurrentPP { get; set; }
		public int MaxPP { get; set; }
		public string Name{ get; set; }
		// public Category -> Physical, Special or Status
		
		protected Random randomAccuracy = new Random( );
		//protected Random randomCritHit = new Random( );
		//protected Random randomStatusEffect = new Random( );
		protected int oddsOfCritHit;
		protected int oddsOfAccuracy;
		protected int oddsOfStatusEffect;
		
		public override string ToString( )
		{
			return Name;
		}
		
		public void SubtractPP( int amount )
		{
			CurrentPP -= amount;
			if( CurrentPP < 0 ) CurrentPP = 0;
		
			WriteLine( $"{this.Name} has {CurrentPP} PP remaining." );
			WriteLine( );
		}
		
		public Moves( string name, string category, Types type, int currentPP, int maxPP, int priority )
		{
			Name = name;
			Category = category;
			Type = type;
			CurrentPP = currentPP;
			MaxPP = maxPP;
			Priority = priority;
			
			oddsOfCritHit = randomAccuracy.Next( 10000 );
			oddsOfAccuracy = randomAccuracy.Next( 10000 );
			oddsOfStatusEffect = randomAccuracy.Next( 10000 );
		}
		
		public void NewRandomValues( )
		{
			oddsOfCritHit = randomAccuracy.Next( 10000 );
			oddsOfAccuracy = randomAccuracy.Next( 10000 );
			oddsOfStatusEffect = randomAccuracy.Next( 10000 );
		}
		
		public abstract void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName );
		
		public void GenericRecoverHealth( Pokemon attacker, Player attackerName )
		{
			attacker.Health += attacker.MaxHealth / 2;
			if( attacker.Health > attacker.MaxHealth ) attacker.Health = attacker.MaxHealth;
			WriteLine( $"{attackerName}'s {attacker} has recovered health! {attacker} is now has {attacker.Health} health left." );
			
			this.SubtractPP( 1 );
		}
		
		public double GenericAttackMove( Types moveType, Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName, double moveDamage )
		{
			if( oddsOfAccuracy < attacker.statMultipliers[ attacker.AccuracyStage ] * 10000 )
			{
				if( oddsOfCritHit < 625 ) return GenericAttackMoveExecutor( moveType, attacker, defender, attackerName, defenderName, moveDamage, true );
				else return GenericAttackMoveExecutor( moveType, attacker, defender, attackerName, defenderName, moveDamage, false );
			}
			else WriteLine( $"Unlucky! {this.Name} has missed!" );
			return 0;
		}
		
		public double GenericAttackMoveSpecialCrit( Types moveType, Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName, double moveDamage, double critRange )
		{
			if( oddsOfAccuracy < attacker.statMultipliers[ attacker.AccuracyStage ] * 10000 )
			{
				if( oddsOfCritHit < critRange * 2 ) return GenericAttackMoveExecutor( moveType, attacker, defender, attackerName, defenderName, moveDamage, true );
				else return GenericAttackMoveExecutor( moveType, attacker, defender, attackerName, defenderName, moveDamage, false );
			}
			else WriteLine( $"Unlucky! {this.Name} has missed!" );
			return 0;
		}
		// When getting oddsOfAccuracy's random number, multiply it by attacker.statMultipliers[ attacker.AccuracyStage ] 
		public double GenericAttackMoveAccuracy( Types moveType, Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName, double moveDamage, double accuracy )
		{
			if( oddsOfAccuracy < accuracy * 100 * attacker.statMultipliers[ attacker.AccuracyStage ] ) 
			{
				return this.GenericAttackMove( moveType, attacker, defender, attackerName, defenderName, moveDamage );
			}
			else WriteLine( $"Unlucky! {this.Name} has missed!" );
			return 0;
		}
		
		public double GenericAttackMoveExecutor( Types moveType, Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName, double moveDamage, bool isCritHit )
		{
			double damage = moveDamage;
			if( this.Category.Equals("Physical") ) damage *= attacker.AttackDamage( ) / defender.DefenseDamage( );
			else damage *= attacker.SpAttackDamage( ) / defender.SpDefenseDamage( );
			
			damage *= ( 0.4 * attacker.Level + 2 );
			damage /= 50;
			damage += 2;
			damage *= (randomAccuracy.Next( 16 ) + 85.0) / 100.0 ;
			// ((((0.4 * Level + 2) * Power * A / D) / 50 ) + 2 ) * modifier
			// modifier = targets * weather * badge * critical * random ( 85-100) * stab * type * burn 
			double effectiveness = this.Effectiveness( defender.Type, defender.DualType, true );
			
			if( moveType.StabEffect( attacker.Type ) ) damage *= 1.5;
			else if( moveType.StabEffect( attacker.DualType ) ) damage *= 1.5;
			
			damage *= effectiveness;
			
			if( damage > 0 )
			{
				if( isCritHit )
				{
					WriteLine( $"Critical Hit!!" );
					damage *= 2;
				}
				
				defender.Health -= damage;
				if( defender.Health < 0 )
				{
					damage += defender.Health;
					defender.Health = 0;
				}
				DisplayHealth( attacker, defender, attackerName, defenderName, damage );
			}
			return damage;
		}
		
		public void DisplayHealth( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName, double damage )
		{
			WriteLine( $"{attackerName}'s {attacker} has done {damage:F3} damage." );
			WriteLine( $"{defenderName}'s {defender} has {defender.Health:F3} health left." );
		}
		
		public double Effectiveness( Types defenderType, Types defenderDualType, bool wantsWriteLine )
		{
			double effectiveness = this.Type.CheckTypes( defenderType );
			effectiveness *= this.Type.CheckTypes( defenderDualType );
			
			if( wantsWriteLine )
			{
				if( effectiveness == 4 ) WriteLine( "Extremely Super Effective!" );
				else if( effectiveness == 2 ) WriteLine( "Super Effective!" );
				else if( effectiveness == 0.5 ) WriteLine( "Not Very Effective!" );
				else if( effectiveness == 0.25 ) WriteLine( "Extremely Not Effective!" );
				else if( effectiveness == 0 ) WriteLine( $"Does not have any effect..." );
			} 
			return effectiveness;
		}
		
		public void MultiHit( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName, double damage, int hits )
		{
			for( int i = 0; i < hits; i ++ )
			{
				Effectiveness( defender.Type, defender.DualType, true );
				defender.Health -= damage;
				DisplayHealth( attacker, defender, attackerName, defenderName, damage );
			}
		}
		
		public double MoveAccuracy( Pokemon attacker, Pokemon defender )
		{
			return oddsOfAccuracy * attacker.statMultipliers[ attacker.AccuracyStage ] * defender.statMultipliers[ defender.EvasionStage ];
		}
	}
	
}

