using System;
using PokemonTypes;
using PokemonMoves;
using PokemonBattle;
using ActualMoves;

namespace Pokemons
{

// Healths are doubled
	class Bulbasaur : Pokemon
	{
		public Bulbasaur( ) : base( 1, "Bulbasaur", new Types("Grass"), new Types("Poison"), 90, 45, 49, 49, 65, 65, 45, new Moves[ 4 ]{ new SleepPowder( ), new MegaDrain( ), new TakeDown( ), new Mimic( ) } ){ }
	}
		
	class Venusaur : Pokemon
	{
		public Venusaur( ) : base( 3, "Venusaur", new Types("Grass"), new Types("Poison"), 84, 80, 82, 83, 100, 100, 80, new Moves[ 4 ]{ new LeechSeed( ), new SolarBeam( ), new Toxic( ), new RazorLeaf( ) } ){ }
	}
	
	class Charmander : Pokemon
	{
		public Charmander( ) : base( 4, "Charmander", new Types("Fire"), null, 90, 39, 52, 43, 50, 50, 65, new Moves[ 4 ]{ new Dig( ), new Ember( ), new Slash( ), new SwordsDance( ) } ){ } 
	}
	
	class Charizard : Pokemon 
	{
		public Charizard( ) : base( 6, "Charizard", new Types("Fire"), new Types("Flying"), 84, 78, 84, 78, 85, 85, 100, new Moves[ 4 ]{ new FlameThrower( ), new BellyDrum( ), new Roost( ), new DragonClaw( ) } ){ } 
	}
	
	class Squirtle : Pokemon
	{
		public Squirtle( ) : base( 7, "Squirtle", new Types("Water"), null, 90, 44, 48, 65, 50, 50, 43, new Moves[ 4 ]{ new Withdraw( ), new MegaKick( ), new BubbleBeam( ), new SeismicToss( ) } ){ }
	}
	
	class Blastoise : Pokemon
	{
		public Blastoise( ) : base( 9, "Blastoise", new Types("Water"), null, 84, 79, 83, 100, 85, 85, 78,  new Moves[ 4 ]{ new Harden( ), new HydroPump( ), new Rest( ), new IceBeam( ) } ){ }
	}
	
	class Pidgey : Pokemon
	{
		public Pidgey( ) : base( 16, "Pidgey", new Types("Flying"), new Types("Normal"), 100, 40, 45, 40, 35, 35, 56, new Moves[ 4 ] { new Fly( ), new QuickAttack( ), new SandAttack( ), new Whirlwind( ) } ){ }
	}
	
	class Pikachu : Pokemon 
	{
		public Pikachu( ) : base( 25, "Pikachu", new Types("Electric"), null, 100, 35, 55, 30, 50, 50, 90, new Moves[ 4 ]{ new Thunder( ), new ThunderWave( ), new IronTail( ), new LightScreen( ) } ){ }
	}
	
	class Raichu : Pokemon
	{
		public Raichu( ) : base( 26, "Raichu", new Types("Electric"), null, 84, 60, 90, 55, 90, 90, 100, new Moves[ 4 ]{ new Surf( ), new BodySlam( ), new Thunderbolt( ), new Fly( ) } ){ }
	}
	
	class Nidoking : Pokemon
	{
		public Nidoking( ) : base( 34, "Nidoking", new Types("Ground"), new Types("Poison"), 84, 81, 92, 77, 75, 75, 85, new Moves[ 4 ]{ new Fissure( ), new Earthquake( ), new Thunderbolt( ), new Blizzard( ) } ){ }
	}
	
	class Vulpix : Pokemon
	{
		public Vulpix( ) : base( 37, "Vulpix", new Types("Fire"), null, 90, 38, 41, 40, 65, 65, 65, new Moves[ 4 ]{ new Dig( ), new Roar( ), new FlameThrower( ), new ConfuseRay( ) } ) { }
	}
	
	class Ninetales : Pokemon
	{
		public Ninetales( ) : base( 38, "Ninetales", new Types("Fire"), null, 84, 73, 76, 75, 100, 100, 100, new Moves[ 4 ]{ new FireBlast( ), new FireSpin( ), new ConfuseRay( ), new BodySlam( ) } ){ }
	}
	
	class Mankey : Pokemon
	{
		public Mankey( ) : base( 56, "Mankey", new Types("Fighting"), null, 100, 40, 80, 35, 35, 35, 70, new Moves[ 4 ] { new SeismicToss( ), new Dig( ), new Submission( ), new Thunder( ) } ){ }
	}
	
	class Arcanine : Pokemon
	{
		public Arcanine( ) : base( 59, "Arcanine", new Types("Fire"), null, 80, 90, 110, 80, 80, 80, 95, new Moves[ 4 ]{ new FireBlast( ), new BodySlam( ), new Mimic( ), new Reflect( ) } ) { }
	}
	
	class Alakazam : Pokemon
	{
		public Alakazam( ) : base( 65, "Alakazam", new Types("Psychic"), null, 80, 55, 50, 45, 135, 135, 120, new Moves[ 4 ]{ new Psychic( ), new Recover( ), new Reflect( ), new ThunderWave( ) } ){ }
	}
	
	class Golem : Pokemon
	{
		public Golem( ) : base( 76, "Golem", new Types("Ground"), new Types("Rock"), 80, 80, 110, 130, 55, 55, 45, new Moves[ 4 ]{ new BodySlam( ), new Earthquake( ), new RockSlide( ), new Explosion( ) } ){ }
	}
	
	class Slowbro : Pokemon
	{
		public Slowbro( ) : base( 80, "Slowbro", new Types("Water"), new Types("Psychic"), 80, 95, 75, 110, 80, 80, 30, new Moves[ 4 ]{ new Amnesia( ), new ThunderWave( ), new Rest( ), new Surf( ) } ){ }
	}
	
	class Dodrio : Pokemon
	{
		public Dodrio( ) : base( 85, "Dodrio", new Types("Normal"), new Types("Flying"), 84, 60, 110, 70, 60, 60, 100, new Moves[ 4 ]{ new DrillPeck( ), new Mimic( ), new HyperBeam( ), new Toxic( ) } ){ }			
	}
	
	class Dewgong : Pokemon
	{
		public Dewgong( ) : base( 87, "Dewgong", new Types("Water"), new Types("Ice"), 84, 90, 70, 80, 95, 95, 70, new Moves[ 4 ]{ new AuroraBeam( ), new Surf( ), new Mimic( ), new HornDrill( ) } ){ }
	}
	
	class Cloyster : Pokemon
	{
		public Cloyster( ) : base( 91, "Cloyster", new Types("Water"), new Types("Ice"), 80, 50, 95, 180, 85, 85, 70, new Moves[ 4 ]{ new Explosion( ), new Clamp( ), new Blizzard( ), new Surf( ) } ){ }
	}
	
	class Gengar : Pokemon
	{
		public Gengar( ) : base( 94, "Gengar", new Types("Ghost"), new Types("Poison"), 80, 60, 65, 60, 130, 130, 110, new Moves[ 4 ]{ new Hypnosis( ), new MegaDrain( ), new Thunderbolt( ), new Explosion( ) } ){ } 
	}
	
	class Exeggutor : Pokemon
	{
		public Exeggutor( ) : base( 103, "Exeggutor", new Types("Grass"), new Types("Psychic"), 80, 95, 95, 85, 125, 125, 55, new Moves[ 4 ]{ new SleepPowder( ), new Psychic( ), new Explosion( ), new MegaDrain( ) } ){ } 
	}
	
	class Lickitung : Pokemon
	{
		public Lickitung( ) : base( 108, "Lickitung", new Types("Normal"), null, 80, 90, 55, 75, 60, 60, 30, new Moves[ 4 ]{ new Fissure( ), new MegaKick( ), new Rest( ), new Supersonic( ) } ){ } 
	}
	
	class Rhydon : Pokemon
	{
		public Rhydon( ) : base( 112, "Rhydon", new Types("Ground"), new Types("Rock"), 80, 105, 130, 120, 45, 45, 40, new Moves[ 4 ]{ new Earthquake( ), new RockSlide( ), new Substitute( ), new BodySlam( ) } ){ }
	}
	
	class Chansey : Pokemon
	{
		public Chansey( ) : base( 113, "Chansey", new Types("Normal"), null, 74, 250, 5, 5, 105, 105, 50, new Moves[ 4 ]{ new SoftBoiled( ), new IceBeam( ), new ThunderWave( ), new Thunderbolt( ) } ){ }
	}
	
	class Starmie : Pokemon
	{
		public Starmie( ) : base( 121, "Starmie", new Types("Water"), new Types("Psychic"), 80, 60, 75, 85, 100, 100, 115, new Moves[ 4 ]{ new Blizzard( ), new Thunderbolt( ), new Recover( ), new ThunderWave( ) } ){ }
	}
	
	class Scyther : Pokemon
	{
		public Scyther( ) : base( 123, "Scyther", new Types("Bug"), new Types("Flying"), 84, 70, 110, 80, 55, 55, 105, new Moves[ 4 ]{ new Slash( ), new SwordsDance( ), new Agility( ), new HyperBeam( ) } ){ }
	}
	
	class Jynx : Pokemon
	{
		public Jynx( ) : base( 124, "Jynx", new Types("Ice"), new Types("Psychic"), 80, 65, 50, 35, 95, 95, 95, new Moves[ 4 ]{ new LovelyKiss( ), new Blizzard( ), new Psychic( ), new Mimic( ) } ) { }
	}
	
	class Tauros : Pokemon
	{
		public Tauros( ) : base ( 128, "Tauros", new Types("Normal"), null, 80, 75, 100, 95, 70, 70, 110, new Moves[ 4 ]{ new BodySlam( ), new HyperBeam( ), new Earthquake( ), new Blizzard( ) } ){ }
	}
	
	class Magikarp : Pokemon
	{
		public Magikarp( ) : base( 129, "Magikarp", new Types("Water"), null, 100, 20, 10, 55, 20, 20, 80, new Moves[ 3 ]{ new DragonRage( ), new Splash( ), new Tackle( ) } ){ }
	}
	
	class Gyarados : Pokemon
	{
		public Gyarados( ) : base( 130, "Gyarados", new Types("Water"), new Types("Flying"), 84, 95, 125, 79, 100, 100, 81, new Moves[ 4 ]{ new Blizzard( ), new Thunderbolt( ), new Surf( ), new HyperBeam( ) } ){ }
	}
	
	class Lapras : Pokemon
	{
		public Lapras( ) : base( 131, "Lapras", new Types("Ice"), new Types("Water"), 80, 130, 85, 80, 95, 95, 60, new Moves[ 4 ]{ new IceBeam( ), new Thunderbolt( ), new Rest( ), new ConfuseRay( ) } ){ }
	}
	
	class Vaporeon : Pokemon
	{
		public Vaporeon( ) : base( 134, "Vaporeon", new Types("Water"), null, 84, 130, 65, 60, 110, 110, 65, new Moves[ 4 ]{ new AcidArmor( ), new Mimic( ), new HydroPump( ), new IceBeam( ) } ) { }
	}
	
	class Jolteon : Pokemon
	{
		public Jolteon( ) : base( 135, "Jolteon", new Types("Electric"), null, 80, 65, 65, 60, 110, 110, 130, new Moves[ 4 ]{ new Thunderbolt( ), new ThunderWave( ), new PinMissile( ), new SandAttack( ) } ){ }
	}
	
	class Flareon : Pokemon
	{
		// Add focus energy
		public Flareon( ) : base( 136, "Flareon", new Types("Fire"), null, 84, 65, 130, 60, 110, 110, 65, new Moves[ 4 ]{ new FireBlast( ), new DoubleEdge( ), new SandAttack( ), new HyperBeam( ) } ) { }
	}
	
	class Snorlax : Pokemon
	{
		public Snorlax( ) : base( 143, "Snorlax", new Types("Normal"), null, 78, 160, 110, 65, 65, 65, 30, new Moves[ 4 ]{ new BodySlam( ), new HyperBeam( ), new Earthquake( ), new SelfDestruct( ) } ) { }
	}
	
	class Articuno : Pokemon
	{
		public Articuno( ) : base( 144, "Articuno", new Types("Ice"), new Types("Flying"), 80, 90, 85, 100, 125, 125, 85, new Moves[ 4 ]{ new Blizzard( ), new Agility( ), new Rest( ), new HyperBeam( ) } ) { }
	}
	
	class Zapdos : Pokemon
	{
		public Zapdos( ) : base( 145, "Zapdos", new Types("Electric"), new Types("Flying"), 80, 90, 90, 85, 125, 125, 100, new Moves[ 4 ]{ new Thunderbolt( ), new DrillPeck( ), new ThunderWave( ), new Agility( ) } ) { }
	}
	
	class Moltres : Pokemon 
	{
		public Moltres( ) : base( 146, "Moltres", new Types("Fire"), new Types("Flying"), 80, 90, 100, 90, 125, 125, 90, new Moves[ 4 ]{ new Agility( ), new FireBlast( ), new FireSpin( ), new HyperBeam( ) } ) { }
	}
	
	class Dratini : Pokemon
	{
		public Dratini( ) : base( 147, "Dratini", new Types("Dragon"), null, 90, 41, 64, 45, 50, 50, 50, new Moves[ 4 ]{ new DragonRage( ), new Wrap( ), new IceBeam( ), new FireBlast( ) } ) { }
	}
	
	class Dragonite : Pokemon
	{
		public Dragonite( ) : base( 149, "Dragonite", new Types("Dragon"), new Types("Flying"), 84, 91, 134, 95, 100, 100, 80,  new Moves[ 4 ]{ new Agility( ), new Wrap( ), new HyperBeam( ), new Blizzard( ) } ) { }
	}
	
	class Mewtwo : Pokemon
	{
		public Mewtwo( ) : base( 150, "Mewtwo", new Types("Psychic"), null, 68 ,106, 110, 90, 154, 154, 130,  new Moves[ 4 ]{ new Amnesia( ), new Psychic( ), new IceBeam( ), new Recover( ) } ) { }
	}
	
	class Mew : Pokemon
	{
		public Mew( ) : base( 151, "Mew", new Types("Psychic"), null, 72, 100, 100, 100, 100, 100, 100, new Moves[ 4 ]{ new SwordsDance( ), new Earthquake( ), new HyperBeam( ), new SoftBoiled( ) } ) { }
	}
}
