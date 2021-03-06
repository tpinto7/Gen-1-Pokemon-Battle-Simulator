using System;
using static System.Console;
using System.Collections;
using System.Collections.Generic;
using PokemonTypes;
using PokemonMoves;
using Pokemons;
using ActualMoves;

namespace PokemonBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            PokemonBattle myBattle = new PokemonBattle( );
            
            while( true )
            {
			Write( "Please enter the number of Pokemon you would like each trainer to battle with: " );
			string response = ReadLine( );
			WriteLine( );

			if( int.TryParse( response, out int val ) )
			{
				if( val >= 1 && val <= 6 )
				{
					myBattle.NumberOfPokemon = val;
					break;
				}
			}
			Write( "Pokemon should be between 1 and 6. (inclusive) " );
	    }
            
            myBattle.CreatePlayers( );
            
            myBattle.WantsRandomPokemon( );
            
            myBattle.CreatePlayersPokemon( );
        
			myBattle.Battle( );
		}
    }
    
    class PokemonBattle : Player
    {
		public int NumberOfPokemon{ get; set; }
		Player playerOne;
		Player playerTwo;
		
		public PokemonBattle( )
		{
			playerOne = new Player( );
			playerTwo = new Player( );
		}
		
		public void CreatePlayers( )
		{
			Write( "Enter Player One's Name: " );
			playerOne.PlayerName = ReadLine( );
			WriteLine( $"Welcome {playerOne.PlayerName}!" );
			WriteLine( );
			
			while( true )
			{
				Write( "Enter Player Two's Name: " );
				playerTwo.PlayerName = ReadLine( );
				
				if( playerTwo.PlayerName.Equals( playerOne.PlayerName ) )
				{
					Write( "Player One and Player Two have the same name! " );
				} 
				else
				{
					WriteLine( $"Welcome {playerTwo.PlayerName}!" );
					WriteLine( );
					break;
				}
			}	
		}
		
		public void WantsRandomPokemon( )
		{
			playerOne.AskIfRandom( );
			playerTwo.AskIfRandom( );
		}
		
		public void CreatePlayersPokemon( )
		{
			playerOne.CreatePokemon( NumberOfPokemon );
			playerTwo.CreatePokemon( NumberOfPokemon );
		}
		
		public void Battle( )
		{
			playerOne.Index = playerOne.StartingPokemon( );
			playerTwo.Index = playerTwo.StartingPokemon( );
			
			while( playerOne.playerPokemons.Count > 0 && playerTwo.playerPokemons.Count > 0 )
			{
				Moves playerOneMove = playerOne.GetMove( playerOne.playerPokemons[ playerOne.Index ] );
				Moves playerTwoMove = playerTwo.GetMove( playerTwo.playerPokemons[ playerTwo.Index ] );
			
				if( playerOneMove != null && playerTwoMove != null && playerOneMove.Priority.CompareTo( playerTwoMove.Priority ) == 0 )
				{
					ExecuteBattleSpeed( playerOneMove, playerTwoMove, playerOne.playerPokemons[ playerOne.Index ] , playerTwo.playerPokemons[ playerTwo.Index ], playerOne, playerTwo);
				}
				
				else
				{
					ExecuteBattleMovePriority( playerOneMove, playerTwoMove, playerOne.playerPokemons[ playerOne.Index ] , playerTwo.playerPokemons[ playerTwo.Index ], playerOne, playerTwo);
				}
				
				if( !CheckHealth( playerOne, playerTwo) )
				{
					playerOne.EndOfRound( playerOne.playerPokemons[ playerOne.Index ], playerTwo.playerPokemons[ playerTwo.Index ], playerOne, playerTwo );
					playerTwo.EndOfRound( playerTwo.playerPokemons[ playerTwo.Index], playerOne.playerPokemons[ playerOne.Index ], playerTwo, playerOne );
				
					CheckHealth( playerTwo, playerOne );
				}
					
				WriteLine( );
			}
			if( playerOne.playerPokemons.Count == 0 ) WriteLine( $"{playerOne} is out of usable Pokemon! {playerTwo} wins!" );
			else WriteLine( $"{playerTwo} is out of usable Pokemon! {playerOne} wins!" );
		}
		
		public void ExecuteBattleMovePriority( Moves playerOneMove, Moves playerTwoMove, Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName)
		{
			if( playerOneMove.Priority.CompareTo( playerTwoMove.Priority ) == 1 )
			{
				BattleExecutor( playerOneMove, attacker, defender, attackerName, defenderName );
				
				if( attacker.Health > 0 && defender.Health > 0 && defenderName.SamePokemon( defender ) ) BattleExecutor( playerTwoMove, defender, attacker, defenderName, attackerName );
			}
			else
			{
				BattleExecutor( playerTwoMove, defender, attacker, defenderName, attackerName );
				
				if( attacker.Health > 0 && defender.Health > 0 && attackerName.SamePokemon( attacker ) ) BattleExecutor( playerOneMove, attacker, defender, attackerName, defenderName );
			}
		}
		
		public void ExecuteBattleSpeed( Moves playerOneMove, Moves playerTwoMove, Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName)
		{
			if( attacker.CurrentSpeed.CompareTo( defender.CurrentSpeed ) == 1 )
			{
				BattleExecutor( playerOneMove, attacker, defender, attackerName, defenderName );
				
				if( attacker.Health > 0 && defender.Health > 0 && defenderName.SamePokemon( defender ) ) BattleExecutor( playerTwoMove, defender, attacker, defenderName, attackerName );
			}
			else
			{
				BattleExecutor( playerTwoMove, defender, attacker, defenderName, attackerName );
				
				if( attacker.Health > 0 && defender.Health > 0 && attackerName.SamePokemon( attacker ) ) BattleExecutor( playerOneMove, attacker, defender, attackerName, defenderName );
			}
		}
		
		public void BattleExecutor( Moves playerMove, Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			if( attacker.BeamCount > 0 )
			{
				WriteLine( $"{attackerName}'s {attacker} is recharging from hyper beam! It cannot attack!" );
				attacker.BeamCount = 0;
				return;
			}
			
			Random randomSuccess = new Random( );
			Random randomAccuracy = new Random( );
			bool avoidedConfusion = false;
	
			if( !playerMove.Name.Equals("Struggle") ) WriteLine( $"{attackerName}'s {attacker} has selected {playerMove.Name}" );

			if( attacker.IsBeamed ) playerMove = new SolarBeam ( );
			
			if( attacker.IsFlying ) playerMove = new Fly( );
	
			foreach( Moves move in attacker.moveSet )
			{
				if( playerMove.Name.Equals( move.Name, StringComparison.OrdinalIgnoreCase ) )
				{
					if( attacker.IsAsleep ) WriteLine( $"{attacker} is fast asleep." );
					
					else if( attacker.IsFrozen ) WriteLine( $"{attacker} is frozen solid!" );
					
					else if( attacker.IsParalyzed && randomSuccess.Next( 100 ) >= 75 ) WriteLine( $"{attacker} is paralyzed. It is unable to attack!" );
					
					else if( attacker.IsConfused )
					{
						WriteLine( $"{attackerName}'s {attacker} is confused!" );
						attacker.AttackingTurnsConfused++;
						if( randomSuccess.Next( 100 ) < 50 )
						{
							WriteLine( $"{attacker} has hurt itself in confusion!" );
							double damage = 40 * attacker.AttackDamage( ) / defender.DefenseDamage( );
							damage *= ( 0.4 * attacker.Level + 2 );
							damage /= 50;
							damage += 2;
							damage *= (randomAccuracy.Next( 16 ) + 85.0) / 100.0 ;
							attacker.Health -= damage;
							WriteLine( $"{attacker} has {attacker.Health:F3} health remaining!" );
							return;
						}
						else avoidedConfusion = true;
					}
					
					else if( defender.IsFlying ) 
					{
						WriteLine( $"{attackerName}'s {attacker}'s {move.Name} has missed!" );
						move.SubtractPP( 1 );
					}
					
					else if( defender.IsDig )
					{
						// if opponent chooses swift, transform, or bide it will still hit. 
						WriteLine( $"{attackerName}'s {attacker}'s {move.Name} has missed!" );
						move.SubtractPP( 1 );
					}
					  
					else CallMove( move, attacker, defender, attackerName, defenderName );
					if( avoidedConfusion ) CallMove( move, attacker, defender, attackerName, defenderName );
					
					WriteLine( );
					return;
				}
			}
		}
		public void CallMove( Moves playerMove, Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			WriteLine( );
			playerMove.NewRandomValues( );
			playerMove.ExecuteMove( attacker, defender, attackerName, defenderName );
			if( playerMove.Type.NameType.Equals("Fire") && defender.IsFrozen )
			{
				WriteLine( $"{defenderName}'s {defender} has been thawed out! {defender} is no longer frozen!" );
			}
		}
	}
    
    class Player : Pokemon
    {
		public string PlayerName{ get; set; }
		public List< Pokemon > playerPokemons;
		public List< int > indexToSkip;
		public bool RandomPokemon{ get; set; }
		public int Index{ get; set; }
		
		public Player( )
		{
			playerPokemons = new List< Pokemon >( );
			indexToSkip = new List< int >( );
		}

		public override string ToString( )
		{
			return PlayerName;
		}

		public void AskIfRandom( )
		{
			//WriteLine( $"{PlayerName}, enter 'yes' if you would like your Pokemon to be random. Enter anything else if you do not." );
			//RandomPokemon = ReadLine( ).Equals( "yes" ) ? true : false; 
			RandomPokemon = true;
		}

		public void CreatePokemon( int numberOfPokemon )
		{
			for( int i = 0; i < numberOfPokemon; i ++ )
			{
				if( !RandomPokemon )playerPokemons.Add( PokemonCreator( PlayerName, i ) );
				else playerPokemons.Add( PokemonCreatorRandom( ref indexToSkip ) );
				
				// WriteLine( $"{PlayerName} has selected {playerPokemons[ i ]}!" );
			}
		}
		
		public int StartingPokemon( )
		{
			WriteLine( $"{PlayerName}, here are your Pokemon:" );
			int i = 0;
			
			foreach( Pokemon pokemon in playerPokemons )
			{
				WriteLine( $"{i++}: {pokemon}" );
			}
			
			WriteLine( );
			while( true )
			{
				Write( $"{PlayerName}, please select the number for the Pokemon you want to send out: " );
				string response = ReadLine( );
				WriteLine( );
				
				if( int.TryParse( response, out int val ) )
				{
					if( val >= 0 && val < playerPokemons.Count ) return val;
				}	
				Write( "Please enter a number less than your total Pokemon. " );		
			}
		}
		
		public Moves GetMove( Pokemon attacker )
		{
			if( attacker.BeamCount == 0 && !attacker.IsBeamed && !attacker.IsFlying && !attacker.IsDig )
			{
				Write($"Choose one of the following moves for {this}'s {attacker}:   * ");
				foreach( Moves move in attacker.moveSet )
				{
					Write( move + " * ");
				}
				WriteLine( );
				
				while( true )
				{
					bool validMove = false;
					string response = ReadLine( );
					
					foreach( Moves move in attacker.moveSet )
					{
						if( response.Equals( move.Name, StringComparison.OrdinalIgnoreCase ) )
						{
							validMove = true;
							if( move.CurrentPP > 0 ) return move;
							else WriteLine( "{move} is out of usable PP!" );
						}
					}
					if( !validMove ) WriteLine( "Please enter a valid move!" );
				}
			}
			return new Struggle( );
		}
		
		public bool SamePokemon( Pokemon pokemon )
		{
			return this.playerPokemons[ this.Index ].Equals( pokemon ); 
		}
		
		public bool CheckHealth( Player playerOne, Player playerTwo )
		{
			bool oneFainted = false;
			bool twoFainted = false;
			if( playerOne.playerPokemons[playerOne.Index].Health <= 0 )
			{
				oneFainted = true;
				WriteLine( $"{playerOne}'s {playerOne.playerPokemons[playerOne.Index]} has fainted." );
				playerOne.playerPokemons.RemoveAt( playerOne.Index );		
			}
			
			if( playerTwo.playerPokemons[playerTwo.Index].Health <= 0 )
			{
				twoFainted = true;
				WriteLine( $"{playerTwo}'s {playerTwo.playerPokemons[playerTwo.Index]} has fainted." );
				playerTwo.playerPokemons.RemoveAt( playerTwo.Index );		
			}
			
			if(( oneFainted || twoFainted ) && playerTwo.playerPokemons.Count > 0 && playerOne.playerPokemons.Count > 0)
			{
				int newPlayerOneIndex = playerOne.StartingPokemon( );
				int newPlayerTwoIndex = playerTwo.StartingPokemon( );
				if( !oneFainted && newPlayerOneIndex != playerOne.Index ) playerOne.playerPokemons[ playerOne.Index ].ResetPokemon( );
				playerOne.Index = newPlayerOneIndex;
				
				if( !twoFainted && newPlayerTwoIndex != playerTwo.Index ) playerTwo.playerPokemons[ playerTwo.Index ].ResetPokemon( );
				playerTwo.Index = newPlayerTwoIndex;
			}
			return ( oneFainted || twoFainted );
		}
		
		public void SwitchRandomPokemon( )
		{
			Random rIndex = new Random( );
			
			if( this.playerPokemons.Count <= 1 ) 
			{
				WriteLine( "Move has failed!" );
				return;
			}
			
			while( true )
			{
				int newIndex = rIndex.Next( this.playerPokemons.Count );
				if( newIndex != this.Index )
				{
					this.Index = newIndex;
					WriteLine( $"{this}'s {playerPokemons[ newIndex ]} has been dragged out!" );
					break;
				}
			}
		}
	}
    
    class Pokemon 
    {
		public double IndexNo{ get; set; }
		public int Level{ get; set; }
		public double MaxHealth{ get; set; }
		public double Health{ get; set; }
		public double Attack{ get; set; }
		public double SpAttack{ get; set; }
		public double Defense{ get; set; }
		public double SpDefense{ get; set; }
		public double Speed{ get; set; }
		public string Name{ get; set; }
		public Types Type{ get; set; }
		public Types DualType{ get; set; }
		public Moves[ ] moveSet;
		
		public bool ImpairedStatus{ get; set; }
		public bool IsPoisoned{ get; set; }
		public bool IsParalyzed{ get; set; }
		public bool IsAsleep{ get; set; }
		public bool IsBurned{ get; set; }
		public bool IsFrozen{ get; set; }
		public bool IsConfused{ get; set; }
		public bool IsLeeched{ get; set; }
		public bool IsBeamed{ get; set; }
		public bool IsFlying{ get; set; }
		public bool IsDig{ get; set; }
		public bool IsWrapped{ get; set; }
		public bool IsClamped{ get; set; }
		// fire spin
		// bind
		
		public int SleepCount{ get; set; }
		public int RestCount{ get; set; }
		public int PoisonCount{ get; set; }
		public int BeamCount{ get; set; }
		public int LeechCount{ get; set; }
		public double WrapDamage{ get; set; }
		public double ClampDamage{ get; set; }
		public int WrapCount{ get; set; }
		public int ClampCount{ get; set; }
		
		public double[ ] statMultipliers = { 0.25, 0.28, 0.33, 0.4, 0.5, 0.6, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0 }; 
		public int AttackStage{ get; set; }
		public int DefenseStage{ get; set; }
		public int SpAttackStage{ get; set; }
		public int SpDefenseStage{ get; set; }
		public int SpeedStage{ get; set; }
		public int AccuracyStage{ get; set; }
		public int EvasionStage{ get; set; }
		public int AttackingTurnsConfused{ get; set; }
		
		public double CurrentSpeed{ get { return Speed * statMultipliers[ SpeedStage ]; } }
		
		public Pokemon( )
		{
		}
		
		public Pokemon( int index, string name, Types type, Types dualType, int level, double health, double attack, double defense, double spAttack, double spDefense, double speed, Moves[ ] moves)
		{
			IndexNo = index;
			Name = name;
			Type = type;
			DualType = dualType;
			Level = level;
			Health = 110 + 2 * health;
			MaxHealth = 110 + 2 * health;
			Attack = 5 + 2 * attack;
			Defense = 5 + 2 * defense;
			SpAttack = 5 + 2 * spAttack;
			SpDefense = 5 + 2 * spDefense;
			Speed = 5 + 2 * speed;
			moveSet = moves;
			this.ResetPokemon( );
		}
		
		public override string ToString( )
		{
			return Name;
		}
		
		public void ResetPokemon( )
		{
			AttackStage = DefenseStage = SpAttackStage = SpDefenseStage = SpeedStage = AccuracyStage = EvasionStage = 6;
		}
		
		public Pokemon PokemonCreator( string playerName, int i )
		{
			WriteLine( $"{playerName}, please choose your Pokemon {i+1}." );
			while( true )
			{
				WriteLine("You can choose between Charizard, the fire-breathing dragon, blastoise, the ferocious water turtle, venusaur, the powerful plant-creature, pikachu, the iconic electric-mouse, or snorlax, the sleeping giant.");
				string name = ReadLine( );
				WriteLine( );
				
				if( name.Equals( "Charizard", StringComparison.OrdinalIgnoreCase ) ) return new Charizard( );
				
				if( name.Equals( "Venusaur", StringComparison.OrdinalIgnoreCase ) ) return new Venusaur( );
				
				if( name.Equals( "Blastoise" , StringComparison.OrdinalIgnoreCase) ) return new Blastoise( );
				
				if( name.Equals( "Pikachu" , StringComparison.OrdinalIgnoreCase ) ) return new Pikachu( );
				
				if( name.Equals( "Snorlax", StringComparison.OrdinalIgnoreCase ) ) return new Snorlax( );
				name = "";
				WriteLine( "Please enter a valid Pokemon Name. Capitalization does not matter!" );
				WriteLine( );
			}
		}
		
		public Pokemon PokemonCreatorRandom( ref List< int > indexToSkip )
		{
			Random randomIndex = new Random( );
			
			while( true )
			{
				int nextIndex = randomIndex.Next( 151 ) + 1;
				if( IndexToSkip( indexToSkip, nextIndex ) )
				{
					indexToSkip.Add( nextIndex );
					switch( nextIndex )
					{
						case 1 : return new Bulbasaur( );
						case 3 : return new Venusaur( );
						case 4 : return new Charmander( );
						case 6 : return new Charizard( );
						case 7 : return new Squirtle( );
						case 9 : return new Blastoise( );
						case 16 : return new Pidgey( );
						case 25 : return new Pikachu( );
						case 26 : return new Raichu( );
						case 34 : return new Nidoking( );
						case 37 : return new Vulpix( );
						case 38 : return new Ninetales( );
						case 56 : return new Mankey( );
						case 59 : return new Arcanine( );
						case 65 : return new Alakazam( );
						case 76 : return new Golem( );
						case 80 : return new Slowbro( );
						case 85 : return new Dodrio( );
						case 87 : return new Dewgong( );
						case 91 : return new Cloyster( );
						case 94 : return new Gengar( );
						case 103 : return new Exeggutor( );
						case 108 : return new Lickitung( );
						case 112 : return new Rhydon( );
						case 113 : return new Chansey( );
						case 121 : return new Starmie( );
						case 123 : return new Scyther( );
						case 124 : return new Jynx( );
						case 128 : return new Tauros( );
						case 129 : return new Magikarp( );
						case 130 : return new Gyarados( );
						case 131 : return new Lapras( );
						case 134 : return new Vaporeon( );
						case 135 : return new Jolteon( );
						case 136 : return new Flareon( );
						case 143 : return new Snorlax( );
						case 144 : return new Articuno( );
						case 145 : return new Moltres( );
						case 146 : return new Zapdos( );
						case 147 : return new Dratini( );
						case 149 : return new Dragonite( );
						case 150 : return new Mewtwo( );
						case 151 : return new Mew( );
					}
				}
			}
		}
		
		public bool IndexToSkip( List< int > indexToSkip, int nextIndex )
		{
			for( int index = 0; index < indexToSkip.Count; index ++ )
			{
				if( indexToSkip[ index ] == nextIndex ) return false;
			}
			return true;
		}
		
		public void EndOfRound( Pokemon attacker, Pokemon defender, Player attackerName, Player defenderName )
		{
			Random oddsOfStatus = new Random( );
			if( attacker.IsAsleep )
			{
				if( attacker.RestCount > 0 )
				{
					attacker.RestCount--;
					if( attacker.RestCount == 0 )
					{
						WriteLine( $"{attackerName}'s {attacker} has woken up!" ); 
						attacker.IsAsleep = false;
						attacker.ImpairedStatus = false;
					}
				}
				else 
				{
					if( attacker.SleepCount > 0 )
					{
						if( oddsOfStatus.Next( 100 ) < 15 || attacker.SleepCount == 7 )
						{
							WriteLine( $"{attackerName}'s {attacker} has woken up!" ); 
							attacker.IsAsleep = false;
							attacker.ImpairedStatus = false;
							attacker.SleepCount = 0;
						}
					}
					attacker.SleepCount++;
				}
			}
			
			if( attacker.IsPoisoned )
			{
				attacker.PoisonCount++;
				double damage = attacker.PoisonCount / 16.0 * attacker.MaxHealth;

				attacker.Health -= damage;
				WriteLine( $"{attackerName}'s {attacker} has lost {damage:F3} health due to poison! {attacker} has {attacker.Health:F3} health remaining. " );
			}
			
			if( attacker.IsLeeched && defender.Health > 0)
			{
				attacker.LeechCount++;
				double damage = attacker.LeechCount / 16 * attacker.MaxHealth;
				
				WriteLine( $"{defenderName}'s {defender} has leeched {damage:F3} health from {attackerName}'s {attacker}. {attacker} has {attacker.Health:F3} health remaining. {defender} has {defender.Health:F3} health remaining." );
			}
			
			if( attacker.IsBurned )
			{
				double damage = attacker.MaxHealth / 16;
				attacker.Health -= damage;
				WriteLine( $"{attackerName}'s {attacker} has lost {damage} health due to burn! {attacker} has {attacker.Health:F3} health remaining.");
			}
			
			if( attacker.IsFrozen )
			{
				if( oddsOfStatus.Next( 100 ) < 10 )
				{
					attacker.IsFrozen = false;
					attacker.ImpairedStatus = false;
					WriteLine( $"{attackerName}'s {attacker} is no longer frozen!" );
				}
			}
			
			if( attacker.IsConfused )
			{
				if( attacker.AttackingTurnsConfused == 4 )
				{
					attacker.IsConfused = false;
					attacker.AttackingTurnsConfused = 0;
					WriteLine( $"{attackerName}'s {attacker} has snapped out of its confusion!" );
				}
				else if( oddsOfStatus.Next( 100 ) < 33 )
				{
					attacker.IsConfused = false;
					attacker.AttackingTurnsConfused = 0;
					WriteLine( $"{attackerName}'s {attacker} has snapped out of its confusion!" );
				}
			}
			
			if( attacker.IsWrapped )
			{
				// if target switches out, user should use this automatically...
				// user cannot select a move and the target cannot execute a move during the effect.
				attacker.WrapCount++;
				
				if( attacker.WrapCount > 5 )
				{
					attacker.IsWrapped = false;
					attacker.WrapCount = 0;
				}
				
				else if( attacker.WrapCount > 3 && oddsOfStatus.Next( 1000 ) < 125 )
				{
					attacker.IsWrapped = false;
					attacker.WrapCount = 0;
				}
				else if( attacker.WrapCount > 1 && oddsOfStatus.Next( 1000 ) < 375 )
				{
					attacker.IsWrapped = false;
					attacker.WrapCount = 0;
				}
				
				if( attacker.IsWrapped )
				{
					if( attacker.WrapCount == 5 ) attacker.IsWrapped = false;
					attacker.Health -= attacker.WrapDamage;
					if( attacker.Health < 0 ) attacker.Health = 0;
					WriteLine( $"{attackerName}'s {attacker} has lost {attacker.WrapDamage:F3} health due to wrap! {attacker} has {attacker.Health:F3} health remaining.");
				}
				if( !attacker.IsWrapped ) WriteLine( $"{attackerName}'s {attacker} is no longer wrapped!" );
			}  
		}
		
		public void HasBurn( )
		{
			WriteLine( $"{this} has been burned!" );
			this.ImpairedStatus = false;
			this.IsBurned = true;
			this.Attack /= 2;
		}
		
		public void HasParalyz( )
		{
			this.IsParalyzed = true;
			this.ImpairedStatus = true;
			this.Speed /= 2;
			WriteLine( $"{this} has been paralyzed!" );
		}
		
		public void HasFrozen( )
		{
			this.IsFrozen = true;
			this.ImpairedStatus = true;
			WriteLine( $"{this} has frozen!" );
		}
		
		public double AttackDamage( ) => this.Attack * this.statMultipliers[ this.AttackStage ];	
		public double SpAttackDamage( ) => this.SpAttack * this.statMultipliers[ this.SpAttackStage ];	
		public double DefenseDamage( ) => this.Defense * this.statMultipliers[ this.DefenseStage ];	
		public double SpDefenseDamage( ) => this.SpDefense * this.statMultipliers[ this.SpDefenseStage ];	
		
		public int CheckMultipliers( string name, int stage )
		{
			if( stage >= 12 )
			{
				stage = 12;
				WriteLine( $"{this}'s {name} cannot go any higher!" );
			}
			if( stage <= 0 )
			{
				stage = 0;
				WriteLine( $"{this}'s {name} cannot go any lower!" );
			}
			return stage;
		}
		
		public bool TypeCheck( string type )
		{
			if( this.Type.NameType.Equals( type ) ) return true;
			
			else if( this.DualType == null ) return false;
			else if( this.DualType.NameType.Equals( type ) ) return true;
			
			return false;
		}
	}
	
}
