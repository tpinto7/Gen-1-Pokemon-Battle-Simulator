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
	}
	
	class FlameThrower : Moves 
	{
		public FlameThrower( ) : base( "Flame Thrower", "Special", new Types("Fire"), 15, 24, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 95 );
		
			if( oddsOfStatusEffect < 1000 && defender.ImpairedStatus == false  && !defender.TypeCheck("Fire") ) defender.HasBurn( );
			base.SubtractPP( 1 );
		}
	}
	
	class BellyDrum : Moves // not gen 1
	{
		public BellyDrum( ) : base( "Belly Drum", "Status", new Types("Normal"), 16, 16, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			attacker.Health -= attacker.MaxHealth / 2;
			attacker.AttackStage += 4;
			WriteLine( $"{attackerName}'s {attacker} has greatly increased its attack!" );
			WriteLine( $"{attacker} now has {attacker.Health} health left." );
			
			base.SubtractPP( 1 );
		}
	}
	
	class Roost : Moves // not gen 1
	{
		public Roost( ) : base( "Roost", "Status", new Types("Flying"), 16, 16, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			GenericRecoverHealth( attacker, attackerName );
		}
	}
	
	class DragonClaw : Moves // not gen 1
	{
		public DragonClaw( ) : base( "Dragon Claw", "Physical", new Types("Dragon"), 16, 16, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 80 );
			
			if( oddsOfStatusEffect >= 8000 ) 
			{
				if( defender.DefenseStage > 0 )WriteLine( $"{defenderName}'s {defender}'s defense has fallen!" );
				defender.DefenseStage--;
				defender.DefenseStage = defender.CheckMultipliers( "defense", attacker.DefenseStage );	
			}
			base.SubtractPP( 1 );
		}
	}
	
	class SolarBeam : Moves 
	{
		public SolarBeam( ) : base( "Solar Beam", "Special", new Types("Grass"), 10, 16, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( attacker.IsBeamed ) ExecuteSolarBeam( attacker, defender, attackerName, defenderName );
			else
			{
				attacker.IsBeamed = true;
				WriteLine( $"{attackerName}'s {attacker} is charging up!" );
			}
		}
		
		public void ExecuteSolarBeam( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			WriteLine( $"{attackerName}'s {attacker} has used Solar Beam!" );
			attacker.IsBeamed = false;
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 120 );
			base.SubtractPP( 1 );
		}
			
	}
	
	class Toxic : Moves
	{
		public Toxic( ) : base( "Toxic", "Status", new Types("Poison"), 10, 16, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( oddsOfAccuracy * attacker.statMultipliers[ attacker.AccuracyStage ] < 8500 )
			{
				if( !defender.ImpairedStatus )
				{
					defender.IsPoisoned = true;
					defender.ImpairedStatus = true;
					WriteLine( $"{defenderName}'s {defender} has been badly poisoned!" );
				}
				else WriteLine( $"{defenderName}'s {defender} already has a status impairment!" );
			}
			else WriteLine( "Unlucky! Toxic has missed!" );
			base.SubtractPP( 1 );
		}
	}
		
	class LeechSeed : Moves
	{
		public LeechSeed( ) : base( "Leech Seed", "Status", new Types("Grass"), 10, 16, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( oddsOfAccuracy * attacker.statMultipliers[ attacker.AccuracyStage ] < 9000 )
			{
				if( defender.TypeCheck( "Grass" ) ) WriteLine( "Leech Seed does not affect grass Pokemon!" ); 
				else
				{
					defender.IsLeeched = true;
					WriteLine( $"{defenderName}'s {defender} has been leeched!" );
				}
			}
			else WriteLine( "Unlucky! Leech Seed has missed!" );
			base.SubtractPP( 1 );
		}
	}
	
	class RazorLeaf : Moves
	{
		public RazorLeaf( ) : base( "Razor Leaf", "Special", new Types("Grass"), 25, 32, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( oddsOfAccuracy * attacker.statMultipliers[ attacker.AccuracyStage ] < 9500 ) base.GenericAttackMoveSpecialCrit( base.Type, attacker, defender, attackerName, defenderName, 55, 2 );
			else WriteLine( "Unlucky! Razor Leaf has missed!");
			base.SubtractPP( 1 );
		}
	}
	
	class Harden : Moves
	{
		public Harden( ) : base( "Harden", "Status", new Types("Normal"), 30, 32, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( attacker.DefenseStage < 12 ) WriteLine( $"{attackerName}'s {attacker} has increased its defense!" );
			attacker.DefenseStage ++;
			attacker.DefenseStage = attacker.CheckMultipliers( "defense", attacker.DefenseStage );
			base.SubtractPP( 1 );
		} 
	}
	
	class IceBeam : Moves
	{
		public IceBeam( ) : base( "Ice Beam", "Special", new Types("Ice"), 10, 16, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 95 );
			if( oddsOfStatusEffect < 1000 && !defender.ImpairedStatus ) defender.HasFrozen( );
			base.SubtractPP( 1 );
		}
	}
	
	class HydroPump : Moves
	{
		public HydroPump( ) : base( "Hydro Pump", "Special", new Types("Water"), 5, 8, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 120, 80 );
			base.SubtractPP( 1 );
		}
	}
	
	class Rest : Moves 
	{
		public Rest( ) : base( "Rest", "Status", new Types("Normal"), 10, 16, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if ( attacker.IsAsleep ) WriteLine($"{attackerName}'s {attacker} is already asleep!" );
			
			else
			{
				attacker.Health = attacker.MaxHealth;
				if( attacker.IsParalyzed ) attacker.Speed *= 2;
				if( attacker.IsBurned ) attacker.Attack *= 2;
				attacker.IsParalyzed = attacker.IsPoisoned = attacker.IsFrozen = attacker.IsBurned = false;
				attacker.IsAsleep = true;
				attacker.RestCount = 3;
				attacker.ImpairedStatus = true;
				
				WriteLine( $"{attacker} has regained full health and recovered from all ailments! {attacker} must sleep for two turns!" );
			}
			base.SubtractPP( 1 );
		}
	}
	
	class IronTail : Moves
	{
		public IronTail( ) : base( "Iron Tail", "Physical", new Types("Steel"), 16, 16, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 90 );
			base.SubtractPP( 1 );
		}
	}
	
	class Thunder : Moves
	{
		public Thunder( ) : base( "Thunder", "Special", new Types("Electric"), 16, 16, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 120, 70 );
			double effectiveness = this.Effectiveness( defender.Type, defender.DualType, false );
			
			if( oddsOfStatusEffect >= 9000 && !defender.ImpairedStatus && effectiveness != 0 ) defender.HasParalyz( );
			base.SubtractPP( 1 );
		}
	}
	
	class ThunderWave : Moves
	{
		public ThunderWave( ) : base( "Thunder Wave", "Status", new Types("Electric"), 20, 32, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( !defender.ImpairedStatus)
			{
				if( defender.TypeCheck("Ground") ) WriteLine( $"Thunder Wave does not affect {defenderName}'s {defender}." );
				
				else defender.HasParalyz( );
			}
			
			else WriteLine( "{defenderName}'s {defender} already has a status impairment!" );
			base.SubtractPP( 1 );
		}
	}
	
	class LightScreen : Moves
	{
		public LightScreen( ) : base( "Light Screen", "Status", new Types("Psychic"), 30, 32, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( attacker.SpDefenseStage < 12 ) WriteLine( $"{attackerName}'s {attacker} has increased its special defense!" );
			attacker.SpDefenseStage += 2;
			attacker.SpDefenseStage = attacker.CheckMultipliers( "special defense", attacker.SpDefenseStage );
			
			base.SubtractPP( 1 );
		}
	}
	
	class BodySlam : Moves 
	{
		public BodySlam( ) : base( "Body Slam", "Physical", new Types("Normal"), 15, 24, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 85 );
			
			double effectiveness = this.Effectiveness( defender.Type, defender.DualType, false );
			
			if( !defender.ImpairedStatus && oddsOfStatusEffect < 3000 && effectiveness != 0  ) defender.HasParalyz( );
			base.SubtractPP( 1 );
		}
	}
	
	class HyperBeam : Moves
	{
		public HyperBeam( ) : base( "Hyper Beam", "Physical", new Types("Normal"), 5, 8, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 150, 90 );
			
			if( defender.Health > 0 && oddsOfAccuracy * attacker.statMultipliers[ attacker.AccuracyStage ] < 9000 )
			{
				WriteLine( $"{attackerName}'s {attacker} must recharge next turn." );
				attacker.BeamCount = 1; 
			}
			base.SubtractPP( 1 );
		}	
	}
	
	class Struggle : Moves
	{
		public Struggle( ) : base( "Struggle", "Physical", new Types("Normal"), 32, 64, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 50 );
		}
	}	
			
	class Earthquake : Moves
	{
		public Earthquake( ) : base( "Earthquake", "Physical", new Types("Ground"), 10, 16, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 100 );
			base.SubtractPP( 1 );
		}
	}
	
	class SelfDestruct : Moves
	{
		public SelfDestruct( ) : base( "Self-Destruct", "Physical", new Types("Normal"), 5, 8, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			defender.Defense /= 2;
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 130 );
			defender.Defense *= 2;
			attacker.Health = 0;
			base.SubtractPP( 1 );
		}	
	}
	
	class Agility : Moves
	{
		public Agility( ) : base( "Agility", "Status", new Types("Psychic"), 30, 32, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( attacker.Speed < 12 ) WriteLine( $"{attackerName}'s {attacker}'s speed has soared!" );
			attacker.SpeedStage += 3;
			attacker.SpeedStage = attacker.CheckMultipliers( "speed", attacker.SpeedStage );
	
			base.SubtractPP( 1 );
		}
	}
	
	class Wrap : Moves
	{
		public Wrap( ) : base( "Wrap", "Physical", new Types("Normal"), 20, 32, 0 ){ } 
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			defender.WrapDamage = base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 15 );
			defender.WrapCount = 0;
			defender.IsWrapped = true;
			base.SubtractPP( 1 );
		}
	}
	
	class Blizzard : Moves
	{
		public Blizzard( ) : base( "Blizzard", "Special", new Types("Ice"), 5, 8, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 120, 90 );
			
			if( oddsOfAccuracy * attacker.statMultipliers[ attacker.AccuracyStage ] < 9000 && oddsOfStatusEffect < 1000 && !defender.ImpairedStatus ) defender.HasFrozen( );
			base.SubtractPP( 1 );
		}
	}
	
	class Thunderbolt : Moves
	{
		public Thunderbolt( ) : base( "Thunderbolt", "Special", new Types("Electric"), 15, 24, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 95 );
			
			double effectiveness = this.Effectiveness( defender.Type, defender.DualType, false);
			
			if( oddsOfStatusEffect >= 9000 && !defender.ImpairedStatus && effectiveness != 0 ) defender.HasParalyz( );
			base.SubtractPP( 1 );
		}
	}
	
	class Surf : Moves
	{
		public Surf( ) : base( "Surf", "Special", new Types("Water"), 15, 24, 0 ){ } 
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 95 );
			base.SubtractPP( 1 );
		}
	}
	
	class Amnesia : Moves
	{
		public Amnesia( ) : base( "Amnesia", "Status", new Types("Psychic"), 20, 32, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( attacker.SpAttackStage < 12 ) WriteLine( $"{attackerName}'s {attacker}'s special attack has greatly risen!" );
			attacker.SpAttackStage += 2;
			attacker.SpAttackStage = attacker.CheckMultipliers( "special attack", attacker.SpAttackStage );
			
			if( attacker.SpDefenseStage < 12 ) WriteLine( $"{attackerName}'s {attacker}'s special defense has greatly risen!" );
			attacker.SpDefenseStage += 2;
			attacker.SpDefenseStage = attacker.CheckMultipliers( "special defense", attacker.SpDefenseStage );
			base.SubtractPP( 1 );
		}
	}
	
	class Psychic : Moves
	{
		public Psychic( ) : base( "Psychic", "Special", new Types("Psychic"), 10, 16, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 90 );
			base.SubtractPP( 1 );
			if( oddsOfStatusEffect < 3300 ) 
			{
				if( defender.SpAttackStage > 0 ) WriteLine( $"{defenderName}'s {defender}'s special attack and defense has fallen!" );
				defender.SpAttackStage--;
				defender.SpAttackStage = defender.CheckMultipliers( "special attack", defender.SpAttackStage );
				
				defender.SpDefenseStage--;
				defender.SpDefenseStage = defender.CheckMultipliers( "special defense", defender.SpDefenseStage );
			}
		}
	}
	
	class Recover : Moves
	{
		public Recover( ) : base( "Recover", "Status", new Types("Normal"), 20, 32, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			GenericRecoverHealth( attacker, attackerName );
		}
	}	
	
	class SoftBoiled : Moves
	{
		public SoftBoiled( ) : base( "Soft-Boiled", "Status", new Types("Normal"), 10, 16, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			GenericRecoverHealth( attacker, attackerName );
		}
	}	
	
	class SwordsDance : Moves
	{
		public SwordsDance( ) : base( "Swords Dance", "Status", new Types("Normal"), 30, 32, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( attacker.AttackStage <= 11 ) WriteLine( $"{attackerName}'s {attacker} has greatly increased its attack!" );
			attacker.AttackStage += 2;
			
			attacker.AttackStage = attacker.CheckMultipliers( "attack", attacker.AttackStage );
			
			base.SubtractPP( 1 );
		}
	}
	
	class Reflect : Moves
	{
		public Reflect( ) : base( "Reflect", "Status", new Types("Psychic"), 20, 32, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( attacker.DefenseStage <= 11 ) WriteLine( $"{attackerName}'s {attacker} has greatly increased its defense!" );
			attacker.DefenseStage += 2;
			
			attacker.DefenseStage = attacker.CheckMultipliers( "defense", attacker.DefenseStage );
			base.SubtractPP( 1 );
		}
	}
	
	class Fly : Moves
	{
		public Fly( ) : base( "Fly", "Physical", new Types("Flying"), 15, 24, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( attacker.IsFlying ) ExecuteFly( attacker, defender, attackerName, defenderName );
			else
			{
				attacker.IsFlying = true;
				WriteLine( $"{attackerName}'s {attacker} has flown high into the sky!" );
			}
		}
		
		public void ExecuteFly( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			WriteLine( $"{attackerName}'s {attacker} has used Fly!" );
			attacker.IsFlying = false;
			base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 70, 95);
			base.SubtractPP( 1 );
		}
		
	}
	
	class QuickAttack : Moves
	{
		public QuickAttack( ) : base( "Quick Attack", "Physical", new Types("Normal"), 30, 32, 1 ){ } 
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			// **Priority 1**
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 40 );
			base.SubtractPP( 1 );
		}
	}
	
	class Whirlwind : Moves
	{
		public Whirlwind( ) : base( "Whirlwind", "Status", new Types("Normal"), 20, 32, 0 ) { }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			defenderName.SwitchRandomPokemon( );
			base.SubtractPP( 1 );
		}
	}
	
	class SandAttack : Moves
	{
		public SandAttack( ) : base( "Sand Attack", "Status", new Types("Ground"), 15, 24, 0 ) { } 
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( defender.AccuracyStage < 12 ) 	WriteLine( $"{defenderName}'s {defender}'s accuracy has fallen!" );
			defender.AccuracyStage--;
			defender.AccuracyStage = defender.CheckMultipliers( "accuracy", defender.AccuracyStage );
			base.SubtractPP( 1 );
		}
	}
	
	class DrillPeck : Moves
	{
		public DrillPeck( ) : base( "Drill Peck", "Physical", new Types("Flying"), 20, 32, 0 ) { }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 80 );
			base.SubtractPP( 1 );
		}
	}
	
	class FireBlast : Moves
	{
		public FireBlast( ) : base( "Fire Blast", "Special", new Types("Fire"), 5, 8, 0 ) { }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( oddsOfAccuracy * attacker.statMultipliers[ attacker.AccuracyStage ] < 8500 )
			{
				base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 120 );
			
				if( oddsOfStatusEffect < 3000 && !defender.ImpairedStatus ) defender.HasBurn( );
			}
			else WriteLine( "Unlucky! Fire Blast has missed" );
			
			base.SubtractPP( 1 );
		}
	}
	
	class FireSpin : Moves
	{
		public FireSpin( ) : base( "Fire Spin", "Special", new Types("Fire"), 15, 24, 0 ) { }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 15, 70 );
			
			if( oddsOfAccuracy * attacker.statMultipliers[ attacker.AccuracyStage ] < 7000 ) WriteLine( ); // defender.IsSpun( )
			base.SubtractPP( 1 );
		}
	}
	
	class RockSlide : Moves
	{
		public RockSlide( ) : base( "Rock Slide", "Physical", new Types("Rock"), 10, 16, 0 ) { }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 75, 90 );
			
			base.SubtractPP( 1 );
		}
	}

	class Explosion : Moves
	{
		public Explosion( ) : base( "Explosion", "Physical", new Types("Normal"), 5, 8, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			defender.Defense /= 2;
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 170 );
			defender.Defense *= 2;
			attacker.Health = 0;
			base.SubtractPP( 1 );
		}	
	}
	
	class Substitute : Moves
	{
		public Substitute( ) : base( "Substitute", "Status", new Types("Normal"), 10, 16, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			attacker.Health -= attacker.MaxHealth / 4.0;
			// Create substitute (attacker.MaxHealth / 4.0 ) + 1
			// Does not protect user from sleep, paralysis, confusion, leech seed, disable
			WriteLine( $"{attackerName}'s {attacker} has placed a substitute!" );
			base.SubtractPP( 1 );
		}
	}
	
	class Absorb : Moves
	{
		public Absorb( ) : base( "Absorb", "Special", new Types("Grass"), 20, 24, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			double damage = base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 20);
			attacker.Health += damage / 2; 
			if( attacker.Health > attacker.MaxHealth ) attacker.Health = attacker.MaxHealth;
			WriteLine( $"{attackerName}'s {attacker} has regained health! {attacker} is now at {attacker.Health} health. " );
			base.SubtractPP( 1 );
		}
	}
	
	class Acid : Moves
	{
		public Acid( ) : base( "Acid", "Physical", new Types("Poison"), 30, 32, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 40);
			
			if( oddsOfStatusEffect < 3300 )
			{
				if( defender.DefenseStage > 0 ) WriteLine( $"{defenderName}'s {defender}'s defense has fallen!" );
				defender.DefenseStage--;
				defender.DefenseStage = defender.CheckMultipliers( "defense", defender.DefenseStage );	
			}
			
			base.SubtractPP( 1 );
		}
	}
	
	class AcidArmor : Moves
	{
		public AcidArmor( ) : base( "Acid", "Status", new Types("Poison"), 40, 48, 0 ){ } 
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( attacker.DefenseStage < 12 ) WriteLine( $"{attackerName}'s {attacker}'s defense has sharply risen!" );
			attacker.DefenseStage += 2;
			attacker.DefenseStage = attacker.CheckMultipliers( "defense", attacker.DefenseStage );
			base.SubtractPP( 1 );
		}
	}
	
	class AuroraBeam : Moves
	{
		public AuroraBeam( ) : base( "Aurora Beam", "Special", new Types("Ice"), 20, 24, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 65);
			
			if( oddsOfStatusEffect < 3300 )
			{
				if( defender.AttackStage > 0 ) 	WriteLine( $"{defenderName}'s {defender}'s attack has fallen!" );
				defender.AttackStage--;
				defender.AttackStage = defender.CheckMultipliers( "attack", defender.DefenseStage );
			}
			
			base.SubtractPP( 1 );
		}
	}
	
	class Barrage : Moves
	{
		public Barrage( ) : base( "Barrage", "Physical", new Types("Normal"), 15, 24, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			// if one of the hits breaks the target's substitute, the move ends
			double damage = base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 15, 85);
			
			if( damage > 0 )
			{
				int hits = 0;
				if( oddsOfStatusEffect < 3750 ) hits = 1;
				else if( oddsOfStatusEffect < 7500 ) hits = 2;
				else if( oddsOfStatusEffect < 8750 ) hits = 3;
				else hits = 4;
			
				MultiHit( attacker, defender, attackerName, defenderName, damage, hits );
			}
			base.SubtractPP( 1 );
		}
	}
	
	class Bite : Moves
	{
		public Bite( ) : base( "Bite", "Physical", new Types("Normal"), 25, 32, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 60);
			
			if( oddsOfStatusEffect < 1000 )
			{
				// defender.IsFlinched
				WriteLine( $"{defenderName}'s {defender} has flinched!" );
			}
			
			base.SubtractPP( 1 );
		}
	}
	
	class BoneClub : Moves
	{
		public BoneClub( ) : base( "Bone Club", "Physical", new Types("Ground"), 20, 32, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			double damage = base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 65, 85);
			
			if( oddsOfStatusEffect < 1000 && damage > 0 )
			{
				// defender.IsFlinched
				WriteLine( $"{defenderName}'s {defender} has flinched!" );
			}
			
			base.SubtractPP( 1 );
		}
	}
	
	class Bonemerang : Moves
	{
		public Bonemerang( ) : base( "Bonemerang", "Physical", new Types("Ground"), 10, 16, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			// if one of the hits breaks the target's substitute, the move ends
			double damage = base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 50, 85);
			if( damage > 0 ) MultiHit( attacker, defender, attackerName, defenderName, damage, 1 );
			base.SubtractPP( 1 );
		}
	}
	
	class Bubble : Moves
	{
		public Bubble( ) : base( "Bubble", "Special", new Types("Water"), 30, 48, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 20);
			
			if( oddsOfStatusEffect < 3300 )
			{
				if( defender.SpeedStage > 0 ) WriteLine( $"{defenderName}'s {defender}'s speed has fallen!" );
				defender.SpeedStage--;
				defender.SpeedStage = defender.CheckMultipliers( "speed", defender.SpeedStage );
			}
			base.SubtractPP( 1 );
		}
	}
	
	class BubbleBeam : Moves
	{
		public BubbleBeam( ) : base( "Bubble Beam", "Special", new Types("Water"), 20, 32, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 65);
			
			if( oddsOfStatusEffect < 3300 )
			{
				if( defender.SpeedStage > 0 ) WriteLine( $"{defenderName}'s {defender}'s speed has fallen!" );
				defender.SpeedStage--;
				defender.SpeedStage = defender.CheckMultipliers( "speed", defender.SpeedStage );
			}
			
			base.SubtractPP( 1 );
		}
	}
	
	class Clamp : Moves
	{
		public Clamp( ) : base( "Clamp", "Special", new Types("Water"), 10, 16, 0 ){ } 
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			// add in wrap functionality
			base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 35, 75 );
			base.SubtractPP( 1 );
		}
	}
	
	class CometPunch : Moves
	{
		public CometPunch( ) : base( "Comet Punch", "Physical", new Types("Normal"), 15, 24, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			// if one of the hits breaks the target's substitute, the move ends
			double damage = base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 18, 85);
			
			if( damage > 0 )
			{
				int hits = 0;
				if( oddsOfStatusEffect < 3750 ) hits = 1;
				else if( oddsOfStatusEffect < 7500 ) hits = 2;
				else if( oddsOfStatusEffect < 8750 ) hits = 3;
				else hits = 4;
			
				MultiHit( attacker, defender, attackerName, defenderName, damage, hits );
			}
			base.SubtractPP( 1 );
		}
	}
	
	class Confusion : Moves
	{
		public Confusion( ) : base( "Confusion", "Special", new Types("Psychic"), 25, 32, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 50);
			
			if( oddsOfStatusEffect < 1000 )
			{
				defender.IsConfused = true;
				WriteLine( $"{defenderName}'s {defender} has become confused!" );
			}
			
			base.SubtractPP( 1 );
		}
	}
	
	class Constrict : Moves
	{
		public Constrict( ) : base( "Constrict", "Physical", new Types("Normal"), 10, 16, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 10);
			
			if( oddsOfStatusEffect < 3300 )
			{
				if( defender.SpeedStage > 0 ) WriteLine( $"{defenderName}'s {defender}'s speed has fallen!" );
				defender.SpeedStage--;
				defender.SpeedStage = defender.CheckMultipliers( "speed", defender.SpeedStage );
			}
			
			base.SubtractPP( 1 );
		}
	}
	
	class Conversion : Moves
	{
		public Conversion( ) : base( "Conversion", "Status", new Types("Normal"), 30, 32, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			attacker.Type = defender.Type;
			attacker.DualType = defender.DualType;
			WriteLine( $"{attackerName}'s {attacker} has taken on the types of {defenderName}'s {defender}." );
		}
	}
	
	class Crabhammer : Moves
	{
		public Crabhammer( ) : base( "Crabhammer", "Special", new Types("Water"), 10, 16, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( oddsOfAccuracy * attacker.statMultipliers[ attacker.AccuracyStage ] < 8500 ) base.GenericAttackMoveSpecialCrit( base.Type, attacker, defender, attackerName, defenderName, 90, 2 );
			else WriteLine( "Unlucky! Crabhammer has missed!");
			base.SubtractPP( 1 );
		}
	}
	
	class Cut : Moves
	{
		public Cut( ) : base( "Cut", "Physical", new Types("Normal"), 30, 48, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 50, 95 );
			base.SubtractPP( 1 );
		}
	}
	
	class DefenseCurl : Moves
	{
		public DefenseCurl( ) : base( "Defense Curl", "Status", new Types("Normal"), 40, 48, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( attacker.AttackStage < 12 ) WriteLine( $"{attackerName}'s {attacker} has increased its defense!" );
			attacker.AttackStage ++;
			attacker.AttackStage = attacker.CheckMultipliers( "attack", attacker.AttackStage );
			base.SubtractPP( 1 );
		} 
	}
	
	class ConfuseRay : Moves
	{
		public ConfuseRay( ) : base( "Confuse Ray", "Status", new Types("Ghost"), 10, 16, 0 ){ }
		
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			defender.IsConfused = true;
			WriteLine( $"{defenderName}'s {defender} has become confused!" );
			base.SubtractPP( 1 );
		}
	}
	
	class Mimic : Moves
	{
		public Mimic( ) : base( "Mimic", "Status", new Types("Normal"), 10, 16, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			// If Pokemon is switched out, should go back to mimic...
			Random randomMove = new Random( );
			Moves newMove = defender.moveSet[ randomMove.Next( 4 ) ];
			
			for( int index = 0; index < attacker.moveSet.Length; index ++ )
			{
				if( attacker.moveSet[ index ].Name.Equals("Mimic") )
				{
					int pp = attacker.moveSet[ index ].CurrentPP;
					attacker.moveSet[ index ] = newMove;
					attacker.moveSet[ index ].CurrentPP = pp;
					WriteLine( $"{attackerName}'s {attacker} has acquired { newMove } from {defenderName}'s {defender}. ");
				}
			}
		}
	}	
	
	class Fissure : Moves
	{
		public Fissure( ) : base( "Fissure", "Physical", new Types("Normal"), 5, 8, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 65535, 30 );
			if( defender.Health == 0 ) WriteLine( "It's a one hit KO!" );
			base.SubtractPP( 1 );
		}
	}
	
	class MegaKick : Moves
	{
		public MegaKick( ) : base( "Mega Kick", "Physical", new Types("Normal"), 5, 8, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 120, 75 );
			base.SubtractPP( 1 );
		}
	}
	
	class Supersonic : Moves
	{
		public Supersonic( ) : base( "Supersonic", "Status", new Types("Normal"), 20, 24, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( oddsOfAccuracy * attacker.statMultipliers[ attacker.AccuracyStage ] < 5500 )
			{
				defender.IsConfused = true;
				WriteLine( $"{defenderName}'s {defender} has become confused!" );
			}
			else WriteLine( "Supersonic has missed!" );
			base.SubtractPP( 1 );
		}
	}	
	
	class MegaDrain : Moves
	{
		public MegaDrain( ) : base( "Mega Drain", "Special", new Types("Grass"), 10, 16, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			double damage = base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 40);
			attacker.Health += damage / 2; 
			if( attacker.Health > attacker.MaxHealth ) attacker.Health = attacker.MaxHealth;
			WriteLine( $"{attackerName}'s {attacker} has regained health! {attacker} is now at {attacker.Health} health. " );
			base.SubtractPP( 1 );
		}
	}
	
	class Hypnosis : Moves
	{
		public Hypnosis( ) : base( "Hypnosis", "Status", new Types("Psychic"), 20, 24, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( !defender.ImpairedStatus )
			{
				if( oddsOfAccuracy * attacker.statMultipliers[ attacker.AccuracyStage ] < 6000 )
				{
					defender.IsAsleep = true;
					defender.ImpairedStatus = true;
					WriteLine( $"{defenderName}'s {defender} has fallen asleep!" );
				}
				else WriteLine( $"{attackerName}'s {attacker}'s hypnosis missed!" );
			}	
			
			else WriteLine( $"{defenderName}'s {defender} already has a status impairment!" );
			base.SubtractPP( 1 );
		}
	}
	
	class SeismicToss : Moves
	{
		public SeismicToss( ) : base( "Seismic Toss", "Physical", new Types("Fighting"), 20, 24, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			double damage = defender.Level;
			defender.Health -= damage;
			DisplayHealth( attacker, defender, attackerName, defenderName, damage );
			base.SubtractPP( 1 );
		}
	}
	
	class Submission : Moves
	{
		public Submission( ) : base( "Submission", "Physical", new Types("Fighting"), 20, 24, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			double damage = base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 40);
			attacker.Health -= damage / 4;
			WriteLine( $"{attackerName}'s {attacker} deals recoil damage to itself. {attacker} is at {attacker.Health} health." );
			base.SubtractPP( 1 );
		}
	}
	
	class Dig : Moves
	{
		public Dig( ) : base( "Dig", "Physical", new Types("Flying"), 20, 24, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( attacker.IsDig ) ExecuteDig( attacker, defender, attackerName, defenderName );
			else
			{
				attacker.IsDig = true;
				WriteLine( $"{attackerName}'s {attacker} has dug deep into the ground!" );
			}
		}
		
		public void ExecuteDig( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			WriteLine( $"{attackerName}'s {attacker} has used Dig!" );
			attacker.IsDig = false;
			base.GenericAttackMove( base.Type, attacker, defender, attackerName, defenderName, 80 );
			base.SubtractPP( 1 );
		}
	}
	
	class Splash : Moves
	{
		public Splash( ) : base( "Splash", "Status", new Types("Normal"), 40, 48, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			WriteLine( $"{attackerName}'s {attacker} splashes around! Nothing happens!" );
			base.SubtractPP( 1 );
		}
	}
	
	class DragonRage : Moves
	{
		public DragonRage( ) : base( "Dragon Rage", "Special", new Types("Dragon"), 10, 16, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			defender.Health -= 40;
			DisplayHealth( attacker, defender, attackerName, defenderName, 40 );
			base.SubtractPP( 1 );
		}
	}
	
	class Tackle : Moves
	{
		public Tackle( ) : base( "Tackle", "Physical", new Types("Normal"), 35, 48, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMoveAccuracy( base.Type, attacker, defender, attackerName, defenderName, 35, 95 );
			base.SubtractPP( 1 );
		}
	}
	
	class Slash : Moves
	{
		public Slash( ) : base( "Slash", "Physical", new Types("Normal"), 20, 24, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			base.GenericAttackMoveSpecialCrit( base.Type, attacker, defender, attackerName, defenderName, 70, 2 );
			base.SubtractPP( 1 );
		}
	}
	
		class SleepPowder : Moves
	{
		public SleepPowder( ) : base( "Sleep Powder", "Status", new Types("Grass"), 15, 24, 0 ){ }
		public override void ExecuteMove( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( !defender.ImpairedStatus )
			{
				if( oddsOfAccuracy * attacker.statMultipliers[ attacker.AccuracyStage ] < 7500 )
				{
					defender.IsAsleep = true;
					defender.ImpairedStatus = true;
					WriteLine( $"{defenderName}'s {defender} has fallen asleep!" );
				}
				else WriteLine( $"{attackerName}'s {attacker}'s sleep powder missed!" );
			}	
			
			else WriteLine( $"{defenderName}'s {defender} already has a status impairment!" );
			base.SubtractPP( 1 );
		}
	}
}

