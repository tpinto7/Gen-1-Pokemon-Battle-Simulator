using System;
using static System.Console;
using System.Collections;
using System.Collections.Generic;

namespace PokemonTypes
{
	class Types
	{
		public string NameType{ get; set; }
	
		public Types( string name )
		{
			NameType = name;
		}
		
		public bool StabEffect( Types attacker )
		{
			if( attacker == null ) return false;
			return( this.NameType.Equals( attacker.NameType ) );
		}
		
		public bool Equals( string name )
		{
			if( this == null ) return false;
			return this.NameType.Equals( name );
		}
		
		public double CheckTypes( Types defender )
		{
			if( defender == null ) return 1;
			
			switch( this.NameType )
			{
				case "Fire" : return FireType( defender );
				case "Water" : return WaterType( defender );
				case "Grass" : return GrassType( defender );
				case "Electric" : return ElectricType( defender );
				case "Ice" : return IceType( defender );
				case "Dragon" : return DragonType( defender );
				case "Normal" : return NormalType( defender );
				case "Fighting" : return FightingType( defender );
				case "Flying" : return FlyingType( defender );
				case "Poison" : return PoisonType( defender );
				case "Ground" : return GroundType( defender );
				case "Rock" : return RockType( defender );
				case "Bug" : return BugType( defender );
				case "Ghost" : return GhostType( defender );
				case "Psychic" : return PsychicType( defender );
			}
			return 1;
		}
		
		public double FireType( Types defender )
		{
			switch( defender.NameType )
			{
				case "Fire" : return 0.5;
				case "Water" : return 0.5;
				case "Grass" : return 2;
				case "Rock" : return 0.5;
				case "Ice" : return 2;
				case "Dragon" : return 0.5;
				default : return 1;
			}
		}
		
		public double WaterType( Types defender )
		{
			switch( defender.NameType )
			{
				case "Fire" : return 2;
				case "Water" : return 0.5;
				case "Grass" : return 0.5;
				case "Ground" : return 2;
				case "Rock" : return 2;
				case "Dragon" : return 0.5;
				default : return 1;
			}
		}
		
		public double GrassType( Types defender )
		{
			switch( defender.NameType )
			{
				case "Flying" : return 0.5;
				case "Poison" : return 0.5;
				case "Ground" : return 2;
				case "Rock" : return 2;
				case "Bug" : return 0.5;
				case "Fire" : return 0.5;
				case "Water" : return 2;
				case "Dragon" : return 0.5;
				default : return 1;
			}
		}
		
		public double ElectricType( Types defender )
		{
			switch( defender.NameType )
			{
				case "Flying" : return 2;
				case "Ground" : return 0;
				case "Water" : return 2;
				case "Grass" : return 0.5;
				case "Electric" : return 0.5;
				case "Dragon" : return 0.5;
				default : return 1;
			}
		}
		
		public double IceType( Types defender )
		{
			switch( defender.NameType )
			{
				case "Flying" : return 2;
				case "Ground" : return 2;
				case "Water" : return 0.5;
				case "Grass" : return 2;
				case "Ice" : return 0.5;
				case "Dragon" : return 2;
				default : return 1;
			}
		}
		
		public double DragonType( Types defender )
		{
			switch( defender.NameType )
			{
				case "Dragon" : return 2;
				default : return 1;
			}
		}
		
		public double NormalType( Types defender )
		{
			switch( defender.NameType )
			{
				case "Rock" : return 0.5;
				case "Ghost" : return 2;
				default : return 1;
			}
		}
		
		public double FightingType( Types defender )
		{
			switch( defender.NameType )
			{
				case "Normal" : return 2;
				case "Flying" : return 0.5;
				case "Poison" : return 0.5;
				case "Rock" : return 2;
				case "Bug" : return 0.5;
				case "Ghost" : return 0;
				case "Psychic" : return 0.5;
				case "Ice" : return 2;
				default : return 1;
			}
		}
		
		public double FlyingType( Types defender )
		{
			switch( defender.NameType )
			{
				case "Flying" : return 2;
				case "Rock" : return 0.5;
				case "Bug" : return 2;
				case "Grass" : return 2;
				case "Electric" : return 0.5;
				default : return 1;
			}
		}
		
		public double PoisonType( Types defender )
		{
			switch( defender.NameType )
			{
				case "Poison" : return 0.5;
				case "Ground" : return 0.5;
				case "Rock" : return 0.5;
				case "Bug" : return 2;
				case "Ghost" : return 0.5;
				case "Grass" : return 2;
				default : return 1;
			}	
		}
		
		public double GroundType( Types defender )
		{
			switch( defender.NameType )
			{
				case "Flying" : return 0;
				case "Poison" : return 2;
				case "Rock" : return 2;
				case "Bug" : return 0.5;
				case "Fire" : return 2;
				case "Grass" : return 0.5;
				case "Electric" : return 2;
				default : return 1;
			}
		}
		
		public double RockType( Types defender )
		{
			switch( defender.NameType )
			{
				case "Fighting" : return 0.5;
				case "Flying" : return 2;
				case "Ground" : return 0.5;
				case "Bug" : return 2;
				case "Fire" : return 2;
				case "Ice" : return 2;
				default : return 1;
			}
		}
		
		public double BugType( Types defender )
		{
			switch( defender.NameType )
			{
				case "Fighting" : return 0.5;
				case "Flying" : return 0.5;
				case "Poison" : return 2;
				case "Fire" : return 0.5;
				case "Green" : return 2;
				case "Psychic" : return 2;
				default : return 1;
			}
		}
		
		public double GhostType( Types defender )				
		{
			switch( defender.NameType )
			{
				case "Normal" : return 0;
				case "Ghost" : return 2;
				case "Psychic" : return 0;
				default : return 1;
			}
		}
		
		public double PsychicType( Types defender )
		{
			switch( defender.NameType )
			{
				case "Fighting" : return 2;
				case "Ground" : return 2;
				case "Psychic" : return 0.5;
				default : return 1;
			}
		}
	}
}

/*class Fire : Types
	{
		public Fire( ) : base( "Fire" )
	}*/
