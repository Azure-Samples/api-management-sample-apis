// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using StarWars.Data.Models;

namespace StarWars.Data;

/// <summary>
/// The Star Wars film dataset (in-memory).
/// </summary>
public class StarWarsData
{
    public StarWarsData()
    {
        // Generate the raw data model
        Films = filmList.ToDictionary(film => film.Id);
        People = personList.ToDictionary(person => person.Id);
        Planets = planetList.ToDictionary(planet => planet.Id);
        Species = speciesList.ToDictionary(species => species.Id);
        Starships = starshipsList.ToDictionary(starship => starship.Id);
        Vehicles = vehiclesList.ToDictionary(vehicle => vehicle.Id);

        // Link up the entities in the data model.
        foreach (int episodeId in Films.Keys)
        {
            foreach (int characterId in CharactersInFilm(episodeId))
            {
                Films[episodeId].Characters.Add(People[characterId]);
                People[characterId].Films.Add(Films[episodeId]);
            }

            foreach (int planetId in PlanetsInFilm(episodeId))
            {
                Films[episodeId].Planets.Add(Planets[planetId]);
                Planets[planetId].Films.Add(Films[episodeId]);
            }

            foreach (int speciesId in SpeciesInFilm(episodeId))
            {
                Films[episodeId].Species.Add(Species[speciesId]);
                Species[speciesId].Films.Add(Films[episodeId]);
            }

            foreach (int starshipId in StarshipsInFilm(episodeId))
            {
                Films[episodeId].Starships.Add(Starships[starshipId]);
                Starships[starshipId].Films.Add(Films[episodeId]);
            }

            foreach (int vehicleId in VehiclesInFilm(episodeId))
            {
                Films[episodeId].Vehicles.Add(Vehicles[vehicleId]);
                Vehicles[vehicleId].Films.Add(Films[episodeId]);
            }
        }

        foreach (int personId in People.Keys)
        {
            int? homeworld = HomeworldForPerson(personId);
            if (homeworld.HasValue)
            {
                People[personId].Homeworld = Planets[homeworld.Value];
            }
        }

        foreach (int speciesId in Species.Keys)
        {
            int? homeworld = HomeworldForSpecies(speciesId);
            if (homeworld.HasValue)
            {
                Species[speciesId].Homeworld = Planets[homeworld.Value];
            }

            foreach (int personId in PeopleInSpecies(speciesId))
            {
                People[personId].Species = Species[speciesId];
            }
        }

        foreach (int starshipId in Starships.Keys)
        {
            foreach (int personId in PilotsForStarship(starshipId))
            {
                Starships[starshipId].Pilots.Add(People[personId]);
                People[personId].Starships.Add(Starships[starshipId]);
            }
        }

        foreach (int vehicleId in Vehicles.Keys)
        {
            foreach (int personId in PilotsForVehicle(vehicleId))
            {
                Vehicles[vehicleId].Pilots.Add(People[personId]);
                People[personId].Vehicles.Add(Vehicles[vehicleId]);
            }
        }
    }

    #region Property Accessors
    /// <summary>
    /// The accessor for the film entities.
    /// </summary>
    public readonly Dictionary<int, Film> Films;

    /// <summary>
    /// The accessor for the person entities.
    /// </summary>
    public readonly Dictionary<int, Person> People;

    /// <summary>
    /// The accessor for the planet entities.
    /// </summary>
    public readonly Dictionary<int, Planet> Planets;

    /// <summary>
    /// The accessor for the species entities.
    /// </summary>
    public readonly Dictionary<int, Species> Species;

    /// <summary>
    /// The accessor for the starship entities.
    /// </summary>
    public readonly Dictionary<int, Starship> Starships;

    /// <summary>
    /// The accessor for the vehicle entities.
    /// </summary>
    public readonly Dictionary<int, Vehicle> Vehicles;
    #endregion

    #region Entity Lists
    /// <summary>
    /// The list of film entities.
    /// </summary>
    private static readonly List<Film> filmList = new()
        {
            new(
                1,
                "The Phantom Menace",
                "Turmoil has engulfed the\r\nGalactic Republic. The taxation\r\nof trade routes to outlying star\r\nsystems is in dispute.\r\n\r\nHoping to resolve the matter\r\nwith a blockade of deadly\r\nbattleships, the greedy Trade\r\nFederation has stopped all\r\nshipping to the small planet\r\nof Naboo.\r\n\r\nWhile the Congress of the\r\nRepublic endlessly debates\r\nthis alarming chain of events,\r\nthe Supreme Chancellor has\r\nsecretly dispatched two Jedi\r\nKnights, the guardians of\r\npeace and justice in the\r\ngalaxy, to settle the conflict....",
                "George Lucas",
                "Rick McCallum",
                new DateOnly(1999, 5, 19)
            ),
            new(
                2,
                "Attack of the Clones",
                "There is unrest in the Galactic\r\nSenate. Several thousand solar\r\nsystems have declared their\r\nintentions to leave the Republic.\r\n\r\nThis separatist movement,\r\nunder the leadership of the\r\nmysterious Count Dooku, has\r\nmade it difficult for the limited\r\nnumber of Jedi Knights to maintain \r\npeace and order in the galaxy.\r\n\r\nSenator Amidala, the former\r\nQueen of Naboo, is returning\r\nto the Galactic Senate to vote\r\non the critical issue of creating\r\nan ARMY OF THE REPUBLIC\r\nto assist the overwhelmed\r\nJedi....",
                "George Lucas",
                "Rick McCallum",
                new DateOnly(2002, 5, 16)
            ),
            new(
                3,
                "Revenge of the Sith",
                "War! The Republic is crumbling\r\nunder attacks by the ruthless\r\nSith Lord, Count Dooku.\r\nThere are heroes on both sides.\r\nEvil is everywhere.\r\n\r\nIn a stunning move, the\r\nfiendish droid leader, General\r\nGrievous, has swept into the\r\nRepublic capital and kidnapped\r\nChancellor Palpatine, leader of\r\nthe Galactic Senate.\r\n\r\nAs the Separatist Droid Army\r\nattempts to flee the besieged\r\ncapital with their valuable\r\nhostage, two Jedi Knights lead a\r\ndesperate mission to rescue the\r\ncaptive Chancellor....",
                "George Lucas",
                "Rick McCallum",
                new DateOnly(2005, 5, 19)
            ),
            new(
                4,
                "A New Hope",
                "It is a period of civil war.\r\nRebel spaceships, striking\r\nfrom a hidden base, have won\r\ntheir first victory against\r\nthe evil Galactic Empire.\r\n\r\nDuring the battle, Rebel\r\nspies managed to steal secret\r\nplans to the Empire's\r\nultimate weapon, the DEATH\r\nSTAR, an armored space\r\nstation with enough power\r\nto destroy an entire planet.\r\n\r\nPursued by the Empire's\r\nsinister agents, Princess\r\nLeia races home aboard her\r\nstarship, custodian of the\r\nstolen plans that can save her\r\npeople and restore\r\nfreedom to the galaxy....",
                "George Lucas",
                "Gary Kurtz, Rick McCallum",
                new DateOnly(1977, 5, 25)
            ),
            new(
                5,
                "The Empire Strikes Back",
                "It is a dark time for the\r\nRebellion. Although the Death\r\nStar has been destroyed,\r\nImperial troops have driven the\r\nRebel forces from their hidden\r\nbase and pursued them across\r\nthe galaxy.\r\n\r\nEvading the dreaded Imperial\r\nStarfleet, a group of freedom\r\nfighters led by Luke Skywalker\r\nhas established a new secret\r\nbase on the remote ice world\r\nof Hoth.\r\n\r\nThe evil lord Darth Vader,\r\nobsessed with finding young\r\nSkywalker, has dispatched\r\nthousands of remote probes into\r\nthe far reaches of space....",
                "George Lucas",
                "Gary Kurtz, Rick McCallum",
                new DateOnly(1980, 5, 17)
            ),
            new(
                6,
                "Return of the Jedi",
                "Luke Skywalker has returned to\r\nhis home planet of Tatooine in\r\nan attempt to rescue his\r\nfriend Han Solo from the\r\nclutches of the vile gangster\r\nJabba the Hutt.\r\n\r\nLittle does Luke know that the\r\nGALACTIC EMPIRE has secretly\r\nbegun construction on a new\r\narmored space station even\r\nmore powerful than the first\r\ndreaded Death Star.\r\n\r\nWhen completed, this ultimate\r\nweapon will spell certain doom\r\nfor the small band of rebels\r\nstruggling to restore freedom\r\nto the galaxy...",
                "Richard Marquand",
                "Howard G. Kazanjian, George Lucas, Rick McCallum",
                new DateOnly(1983, 5, 25)
            )
        };

    /// <summary>
    /// The list of person entities.
    /// </summary>
    private static readonly List<Person> personList = new()
        {
            new Person(1, "Luke Skywalker", 172, 77, "blond", "fair", "blue", -19, Gender.Male),
            new Person(2, "C-3PO", 167, 75, "n/a", "gold", "yellow", -112, Gender.None),
            new Person(3, "R2-D2", 96, 32, "n/a", "white, blue", "red", -33, Gender.None),
            new Person(4, "Darth Vader", 202, 136, "none", "white", "yellow", -41, Gender.Male),
            new Person(5, "Leia Organa", 150, 49, "brown", "light", "brown", -19, Gender.Female),
            new Person(6, "Owen Lars", 178, 120, "brown, grey", "light", "blue", -52, Gender.Male),
            new Person(7, "Beru Whitesun Lars", 165, 75, "brown", "light", "blue", -47, Gender.Female),
            new Person(8, "R5-D4", 97, 32, "n/a", "white, red", "red", null, Gender.None),
            new Person(9, "Biggs Darklighter", 183, 84, "black", "light", "brown", -24, Gender.Male),
            new Person(10, "Obi-Wan Kenobi", 182, 77, "auburn, white", "fair", "blue-gray", -57, Gender.Male),
            new Person(11, "Anakin Skywalker", 188, 84, "blond", "fair", "blue", -41, Gender.Male),
            new Person(12, "Wilhuff Tarkin", 180, null, "auburn, grey", "fair", "blue", -64, Gender.Male),
            new Person(13, "Chewbacca", 228, 112, "brown", "unknown", "blue", -200, Gender.Male),
            new Person(14, "Han Solo", 180, 80, "brown", "fair", "brown", -29, Gender.Male),
            new Person(15, "Greedo", 173, 74, "n/a", "green", "black", -44, Gender.Male),
            new Person(16, "Jabba Desilijic Tiure", 175, 1358, "n/a", "green-tan, brown", "orange", -600, Gender.Hermaphrodite),
            new Person(18, "Wedge Antilles", 170, 77, "brown", "fair", "hazel", -21, Gender.Male),
            new Person(19, "Jek Tono Porkins", 180, 110, "brown", "fair", "blue", null, Gender.Male),
            new Person(20, "Yoda", 66, 17, "white", "green", "brown", -896, Gender.Male),
            new Person(21, "Palpatine", 170, 75, "grey", "pale", "yellow", -82, Gender.Male),
            new Person(22, "Boba Fett", 183, 78, "black", "fair", "brown", -31, Gender.Male),
            new Person(23, "IG-88", 200, 140, "none", "metal", "red", -15, Gender.None),
            new Person(24, "Bossk", 190, 113, "none", "green", "red", -53, Gender.Male),
            new Person(25, "Lando Calrissian", 177, 79, "black", "dark", "brown", -31, Gender.Male),
            new Person(26, "Lobot", 175, 79, "none", "light", "blue", -37, Gender.Male),
            new Person(27, "Ackbar", 180, 83, "none", "brown mottle", "orange", -41, Gender.Male),
            new Person(28, "Mon Mothma", 150, null, "auburn", "fair", "blue", -48, Gender.Female),
            new Person(29, "Arvel Crynyd", null, null, "brown", "fair", "brown", null, Gender.Male),
            new Person(30, "Wicket Systri Warrick", 88, 20, "brown", "brown", "brown", -8, Gender.Male),
            new Person(31, "Nien Nunb", 160, 68, "none", "grey", "black", null, Gender.Male),
            new Person(32, "Qui-Gon Jinn", 193, 89, "brown", "fair", "blue", -92, Gender.Male),
            new Person(33, "Nute Gunray", 191, 90, "none", "mottled green", "red", null, Gender.Male),
            new Person(34, "Finis Valorum", 170, null, "blond", "fair", "blue", -91, Gender.Male),
            new Person(35, "Padmé Amidala", 185, 45, "brown", "light", "brown", -46, Gender.Female),
            new Person(36, "Jar Jar Binks", 196, 66, "none", "orange", "orange", -52, Gender.Male),
            new Person(37, "Roos Tarpals", 224, 82, "none", "grey", "orange", null, Gender.Male),
            new Person(38, "Rugor Nass", 206, null, "none", "green", "orange", null, Gender.Male),
            new Person(39, "Ric Olié", 183, null, "brown", "fair", "blue", null, Gender.Male),
            new Person(40, "Watto", 137, null, "black", "blue, grey", "yellow", null, Gender.Male),
            new Person(41, "Sebulba", 112, 40, "none", "grey, red", "orange", null, Gender.Male),
            new Person(42, "Quarsh Panaka", 183, null, "black", "dark", "brown", -62, Gender.Male),
            new Person(43, "Shmi Skywalker", 163, null, "black", "fair", "brown", -72, Gender.Female),
            new Person(44, "Darth Maul", 175, 80, "none", "red", "yellow", -54, Gender.Male),
            new Person(45, "Bib Fortuna", 180, null, "none", "pale", "pink", null, Gender.Male),
            new Person(46, "Ayla Secura", 178, 55, "none", "blue", "hazel", -48, Gender.Female),
            new Person(47, "Ratts Tyerel", 79, 15, "none", "grey, blue", "unknown", null, Gender.Male),
            new Person(48, "Dud Bolt", 94, 45, "none", "blue, grey", "yellow", null, Gender.Male),
            new Person(49, "Gasgano", 122, null, "none", "white, blue", "black", null, Gender.Male),
            new Person(50, "Ben Quadinaros", 163, 65, "none", "grey, green, yellow", "orange", null, Gender.Male),
            new Person(51, "Mace Windu", 188, 84, "none", "dark", "brown", -72, Gender.Male),
            new Person(52, "Ki-Adi-Mundi", 198, 82, "white", "pale", "yellow", -92, Gender.Male),
            new Person(53, "Kit Fisto", 196, 87, "none", "green", "black", null, Gender.Male),
            new Person(54, "Eeth Koth", 171, null, "black", "brown", "brown", null, Gender.Male),
            new Person(55, "Adi Gallia", 184, 50, "none", "dark", "blue", null, Gender.Female),
            new Person(56, "Saesee Tiin", 188, null, "none", "pale", "orange", null, Gender.Male),
            new Person(57, "Yarael Poof", 264, null, "none", "white", "yellow", null, Gender.Male),
            new Person(58, "Plo Koon", 188, 80, "none", "orange", "black", -22, Gender.Male),
            new Person(59, "Mas Amedda", 196, null, "none", "blue", "blue", null, Gender.Male),
            new Person(60, "Gregar Typho", 185, 85, "black", "dark", "brown", null, Gender.Male),
            new Person(61, "Cordé", 157, null, "brown", "light", "brown", null, Gender.Female),
            new Person(62, "Cliegg Lars", 183, null, "brown", "fair", "blue", -82, Gender.Male),
            new Person(63, "Poggle the Lesser", 183, 80, "none", "green", "yellow", null, Gender.Male),
            new Person(64, "Luminara Unduli", 170, 56, "black", "yellow", "blue", -58, Gender.Female),
            new Person(65, "Barriss Offee", 166, 50, "black", "yellow", "blue", -40, Gender.Female),
            new Person(66, "Dormé", 165, null, "brown", "light", "brown", null, Gender.Female),
            new Person(67, "Dooku", 193, 80, "white", "fair", "brown", -102, Gender.Male),
            new Person(68, "Bail Prestor Organa", 191, null, "black", "tan", "brown", -67, Gender.Male),
            new Person(69, "Jango Fett", 183, 79, "black", "tan", "brown", -66, Gender.Male),
            new Person(70, "Zam Wesell", 168, 55, "blonde", "fair, green, yellow", "yellow", null, Gender.Female),
            new Person(71, "Dexter Jettster", 198, 102, "none", "brown", "yellow", null, Gender.Male),
            new Person(72, "Lama Su", 229, 88, "none", "grey", "black", null, Gender.Male),
            new Person(73, "Taun We", 213, null, "none", "grey", "black", null, Gender.Female),
            new Person(74, "Jocasta Nu", 167, null, "white", "fair", "blue", null, Gender.Female),
            new Person(75, "R4-P17", 96, null, "none", "silver, red", "red, blue", null, Gender.Female),
            new Person(76, "Wat Tambor", 193, 48, "none", "green, grey", "unknown", null, Gender.Male),
            new Person(77, "San Hill", 191, null, "none", "grey", "gold", null, Gender.Male),
            new Person(78, "Shaak Ti", 178, 57, "none", "red, blue, white", "black", null, Gender.Female),
            new Person(79, "Grievous", 216, 159, "none", "brown, white", "green, yellow", null, Gender.Male),
            new Person(80, "Tarfful", 234, 136, "brown", "brown", "blue", null, Gender.Male),
            new Person(81, "Raymus Antilles", 188, 79, "brown", "light", "brown", null, Gender.Male),
            new Person(82, "Sly Moore", 178, 48, "none", "pale", "white", null, Gender.Female),
            new Person(83, "Tion Medon", 206, 80, "none", "grey", "black", null, Gender.Male)
        };

    /// <summary>
    /// The list of planet entities.
    /// </summary>
    private static readonly List<Planet> planetList = new()
        {
            new Planet(1, "Tatooine", 10465, 23, 304, 1, 200000, "arid", "desert", 1),
            new Planet(2, "Alderaan", 12500, 24, 364, 1, 2000000000, "temperate", "grasslands, mountains", 40),
            new Planet(3, "Yavin IV", 10200, 24, 4818, 1, 1000, "temperate, tropical", "jungle, rainforests", 8),
            new Planet(4, "Hoth", 7200, 23, 549, 1.1, null, "frozen", "tundra, ice caves, mountain ranges", 100),
            new Planet(5, "Dagobah", 8900, 23, 341, null, null, "murky", "swamp, jungles", 8),
            new Planet(6, "Bespin", 118000, 12, 5110, 1.5, 6000000, "temperate", "gas giant", 0),
            new Planet(7, "Endor", 4900, 18, 402, 0.85, 30000000, "temperate", "forests, mountains, lakes", 8),
            new Planet(8, "Naboo", 12120, 26, 312, 1, 4500000000, "temperate", "grassy hills, swamps, forests, mountains", 12),
            new Planet(9, "Coruscant", 12240, 24, 368, 1, 1000000000000, "temperate", "cityscape, mountains", null),
            new Planet(10, "Kamino", 19720, 27, 463, 1, 1000000000, "temperate", "ocean", 100),
            new Planet(11, "Geonosis", 11370, 30, 256, 0.9, 100000000000, "temperate, arid", "rock, desert, mountain, barren", 5),
            new Planet(12, "Utapau", 12900, 27, 351, 1, 95000000, "temperate, arid, windy", "scrublands, savanna, canyons, sinkholes", 0.9),
            new Planet(13, "Mustafar", 4200, 36, 412, 1, 20000, "hot", "volcanoes, lava rivers, mountains, caves", 0),
            new Planet(14, "Kashyyyk", 12765, 26, 381, 1, 45000000, "tropical", "jungle, forests, lakes, rivers", 60),
            new Planet(15, "Polis Massa", 0, 24, 590, 0.56, 1000000, "artificial temperate ", "airless asteroid", 0),
            new Planet(16, "Mygeeto", 10088, 12, 167, 1, 19000000, "frigid", "glaciers, mountains, ice canyons", null),
            new Planet(17, "Felucia", 9100, 34, 231, 0.75, 8500000, "hot, humid", "fungus forests", null),
            new Planet(18, "Cato Neimoidia", 0, 25, 278, 1, 10000000, "temperate, moist", "mountains, fields, forests, rock arches", null),
            new Planet(19, "Saleucami", 14920, 26, 392, null, 1400000000, "hot", "caves, desert, mountains, volcanoes", null),
            new Planet(20, "Stewjon", 0, null, null, 1, null, "temperate", "grass", null),
            new Planet(21, "Eriadu", 13490, 24, 360, 1, 22000000000, "polluted", "cityscape", null),
            new Planet(22, "Corellia", 11000, 25, 329, 1, 3000000000, "temperate", "plains, urban, hills, forests", 70),
            new Planet(23, "Rodia", 7549, 29, 305, 1, 1300000000, "hot", "jungles, oceans, urban, swamps", 60),
            new Planet(24, "Nal Hutta", 12150, 87, 413, 1, 7000000000, "temperate", "urban, oceans, swamps, bogs", null),
            new Planet(25, "Dantooine", 9830, 25, 378, 1, 1000, "temperate", "oceans, savannas, mountains, grasslands", null),
            new Planet(26, "Bestine IV", 6400, 26, 680, null, 62000000, "temperate", "rocky islands, oceans", 98),
            new Planet(27, "Ord Mantell", 14050, 26, 334, 1, 4000000000, "temperate", "plains, seas, mesas", 10),
            new Planet(28, "unknown", 0, 0, 0, null, null, "unknown", "unknown", null),
            new Planet(29, "Trandosha", 0, 25, 371, 0.62, 42000000, "arid", "mountains, seas, grasslands, deserts", null),
            new Planet(30, "Socorro", 0, 20, 326, 1, 300000000, "arid", "deserts, mountains", null),
            new Planet(31, "Mon Cala", 11030, 21, 398, 1, 27000000000, "temperate", "oceans, reefs, islands", 100),
            new Planet(32, "Chandrila", 13500, 20, 368, 1, 1200000000, "temperate", "plains, forests", 40),
            new Planet(33, "Sullust", 12780, 20, 263, 1, 18500000000, "superheated", "mountains, volcanoes, rocky deserts", 5),
            new Planet(34, "Toydaria", 7900, 21, 184, 1, 11000000, "temperate", "swamps, lakes", null),
            new Planet(35, "Malastare", 18880, 26, 201, 1.56, 2000000000, "arid, temperate, tropical", "swamps, deserts, jungles, mountains", null),
            new Planet(36, "Dathomir", 10480, 24, 491, 0.9, 5200, "temperate", "forests, deserts, savannas", null),
            new Planet(37, "Ryloth", 10600, 30, 305, 1, 1500000000, "temperate, arid, subartic", "mountains, valleys, deserts, tundra", 5),
            new Planet(38, "Aleen Minor", null, null, null, null, null, "unknown", "unknown", null),
            new Planet(39, "Vulpter", 14900, 22, 391, 1, 421000000, "temperate, artic", "urban, barren", null),
            new Planet(40, "Troiken", null, null, null, null, null, "unknown", "desert, tundra, rainforests, mountains", null),
            new Planet(41, "Tund", 12190, 48, 1770, null, 0, "unknown", "barren, ash", null),
            new Planet(42, "Haruun Kal", 10120, 25, 383, 0.98, 705300, "temperate", "toxic cloudsea, plateaus, volcanoes", null),
            new Planet(43, "Cerea", null, 27, 386, 1, 450000000, "temperate", "verdant", 20),
            new Planet(44, "Glee Anselm", 15600, 33, 206, 1, 500000000, "tropical, temperate", "lakes, islands, swamps, seas", 80),
            new Planet(45, "Iridonia", null, 29, 413, null, null, "unknown", "rocky canyons, acid pools", null),
            new Planet(46, "Tholoth", null, null, null, null, null, "unknown", "unknown", null),
            new Planet(47, "Iktotch", null, 22, 481, 1, null, "arid, rocky, windy", "rocky", null),
            new Planet(48, "Quermia", null, null, null, null, null, "unknown", "unknown", null),
            new Planet(49, "Dorin", 13400, 22, 409, 1, null, "temperate", "unknown", null),
            new Planet(50, "Champala", null, 27, 318, 1, 3500000000, "temperate", "oceans, rainforests, plateaus", null),
            new Planet(51, "Mirial", null, null, null, null, null, "unknown", "deserts", null),
            new Planet(52, "Serenno", null, null, null, null, null, "unknown", "rainforests, rivers, mountains", null),
            new Planet(53, "Concord Dawn", null, null, null, null, null, "unknown", "jungles, forests, deserts", null),
            new Planet(54, "Zolan", null, null, null, null, null, "unknown", "unknown", null),
            new Planet(55, "Ojom", null, null, null, null, 500000000, "frigid", "oceans, glaciers", 100),
            new Planet(56, "Skako", null, 27, 384, 1, 500000000000, "temperate", "urban, vines", null),
            new Planet(57, "Muunilinst", 13800, 28, 412, 1, 5000000000, "temperate", "plains, forests, hills, mountains", 25),
            new Planet(58, "Shili", null, null, null, 1, null, "temperate", "cities, savannahs, seas, plains", null),
            new Planet(59, "Kalee", 13850, 23, 378, 1, 4000000000, "arid, temperate, tropical", "rainforests, cliffs, canyons, seas", null),
            new Planet(60, "Umbara", null, null, null, null, null, "unknown", "unknown", null)
        };

    /// <summary>
    /// The list of species entities.
    /// </summary>
    private static readonly List<Species> speciesList = new()
        {
            new Species(1, "Human", "mammal", "sentient", "180", "120", "blonde, brown, black, red", "caucasian, black, asian, hispanic", "brown, blue, green, hazel, grey, amber", "Galactic Basic"),
            new Species(2, "Droid", "artificial", "sentient", "n/a", "indefinite", "n/a", "n/a", "n/a", "n/a"),
            new Species(3, "Wookie", "mammal", "sentient", "210", "400", "black, brown", "gray", "blue, green, yellow, brown, golden, red", "Shyriiwook"),
            new Species(4, "Rodian", "sentient", "reptilian", "170", "unknown", "n/a", "green, blue", "black", "Galatic Basic"),
            new Species(5, "Hutt", "gastropod", "sentient", "300", "1000", "n/a", "green, brown, tan", "yellow, red", "Huttese"),
            new Species(6, "Yoda's species", "mammal", "sentient", "66", "900", "brown, white", "green, yellow", "brown, green, yellow", "Galactic basic"),
            new Species(7, "Trandoshan", "reptile", "sentient", "200", "unknown", "none", "brown, green", "yellow, orange", "Dosh"),
            new Species(8, "Mon Calamari", "amphibian", "sentient", "160", "unknown", "none", "red, blue, brown, magenta", "yellow", "Mon Calamarian"),
            new Species(9, "Ewok", "mammal", "sentient", "100", "unknown", "white, brown, black", "brown", "orange, brown", "Ewokese"),
            new Species(10, "Sullustan", "mammal", "sentient", "180", "unknown", "none", "pale", "black", "Sullutese"),
            new Species(11, "Neimodian", "unknown", "sentient", "180", "unknown", "none", "grey, green", "red, pink", "Neimoidia"),
            new Species(12, "Gungan", "amphibian", "sentient", "190", "unknown", "none", "brown, green", "orange", "Gungan basic"),
            new Species(13, "Toydarian", "mammal", "sentient", "120", "91", "none", "blue, green, grey", "yellow", "Toydarian"),
            new Species(14, "Dug", "mammal", "sentient", "100", "unknown", "none", "brown, purple, grey, red", "yellow, blue", "Dugese"),
            new Species(15, "Twi'lek", "mammals", "sentient", "200", "unknown", "none", "orange, yellow, blue, green, pink, purple, tan", "blue, brown, orange, pink", "Twi'leki"),
            new Species(16, "Aleena", "reptile", "sentient", "80", "79", "none", "blue, gray", "unknown", "Aleena"),
            new Species(17, "Vulptereen", "unknown", "sentient", "100", "unknown", "none", "grey", "yellow", "vulpterish"),
            new Species(18, "Xexto", "unknown", "sentient", "125", "unknown", "none", "grey, yellow, purple", "black", "Xextese"),
            new Species(19, "Toong", "unknown", "sentient", "200", "unknown", "none", "grey, green, yellow", "orange", "Tundan"),
            new Species(20, "Cerean", "mammal", "sentient", "200", "unknown", "red, blond, black, white", "pale pink", "hazel", "Cerean"),
            new Species(21, "Nautolan", "amphibian", "sentient", "180", "70", "none", "green, blue, brown, red", "black", "Nautila"),
            new Species(22, "Zabrak", "mammal", "sentient", "180", "unknown", "black", "pale, brown, red, orange, yellow", "brown, orange", "Zabraki"),
            new Species(23, "Tholothian", "mammal", "sentient", "unknown", "unknown", "unknown", "dark", "blue, indigo", "unknown"),
            new Species(24, "Iktotchi", "unknown", "sentient", "180", "unknown", "none", "pink", "orange", "Iktotchese"),
            new Species(25, "Quermian", "mammal", "sentient", "240", "86", "none", "white", "yellow", "Quermian"),
            new Species(26, "Kel Dor", "unknown", "sentient", "180", "70", "none", "peach, orange, red", "black, silver", "Kel Dor"),
            new Species(27, "Chagrian", "amphibian", "sentient", "190", "unknown", "none", "blue", "blue", "Chagria"),
            new Species(28, "Geonosian", "insectoid", "sentient", "178", "unknown", "none", "green, brown", "green, hazel", "Geonosian"),
            new Species(29, "Mirialan", "mammal", "sentient", "180", "unknown", "black, brown", "yellow, green", "blue, green, red, yellow, brown, orange", "Mirialan"),
            new Species(30, "Clawdite", "reptilian", "sentient", "180", "70", "none", "green, yellow", "yellow", "Clawdite"),
            new Species(31, "Besalisk", "amphibian", "sentient", "178", "75", "none", "brown", "yellow", "besalisk"),
            new Species(32, "Kaminoan", "amphibian", "sentient", "220", "80", "none", "grey, blue", "black", "Kaminoan"),
            new Species(33, "Skakoan", "mammal", "sentient", "unknown", "unknown", "none", "grey, green", "unknown", "Skakoan"),
            new Species(34, "Muun", "mammal", "sentient", "190", "100", "none", "grey, white", "black", "Muun"),
            new Species(35, "Togruta", "mammal", "sentient", "180", "94", "none", "red, white, orange, yellow, green, blue", "red, orange, yellow, green, blue, black", "Togruti"),
            new Species(36, "Kaleesh", "reptile", "sentient", "170", "80", "none", "brown, orange, tan", "yellow", "Kaleesh"),
            new Species(37, "Pau'an", "mammal", "sentient", "190", "700", "none", "grey", "black", "Utapese")
        };

    /// <summary>
    /// The list of starship entities.
    /// </summary>
    private static readonly List<Starship> starshipsList = new()
        {
            new Starship(2, "CR90 corvette", "corvette", "CR90 corvette", "Corellian Engineering Corporation", 3500000, 150, 950, 30 - 165, 600, 3000000, "1 year", 2.0, 60),
            new Starship(3, "Star Destroyer", "Star Destroyer", "Imperial I-class Star Destroyer", "Kuat Drive Yards", 150000000, 1600, 975, 47060, 0, 36000000, "2 years", 2.0, 60),
            new Starship(5, "Sentinel-class landing craft", "landing craft", "Sentinel-class landing craft", "Sienar Fleet Systems, Cyngus Spaceworks", 240000, 38, 1000, 5, 75, 180000, "1 month", 1.0, 70),
            new Starship(9, "Death Star", "Deep Space Mobile Battlestation", "DS-1 Orbital Battle Station", "Imperial Department of Military Research, Sienar Fleet Systems", 1000000000000, 120000, null, 342953, 843342, 1000000000000, "3 years", 4.0, 10),
            new Starship(10, "Millennium Falcon", "Light freighter", "YT-1300 light freighter", "Corellian Engineering Corporation", 100000, 34.37, 1050, 4, 6, 100000, "2 months", 0.5, 75),
            new Starship(11, "Y-wing", "assault starfighter", "BTL Y-wing", "Koensayr Manufacturing", 134999, 14, 1000, 2, 0, 110, "1 week", 1.0, 80),
            new Starship(12, "X-wing", "Starfighter", "T-65 X-wing", "Incom Corporation", 149999, 12.5, 1050, 1, 0, 110, "1 week", 1.0, 100),
            new Starship(13, "TIE Advanced x1", "Starfighter", "Twin Ion Engine Advanced x1", "Sienar Fleet Systems", null, 9.2, 1200, 1, 0, 150, "5 days", 1.0, 105),
            new Starship(15, "Executor", "Star dreadnought", "Executor-class star dreadnought", "Kuat Drive Yards, Fondor Shipyards", 1143350000, 19000, null, 279144, 38000, 250000000, "6 years", 2.0, 40),
            new Starship(17, "Rebel transport", "Medium transport", "GR-75 medium transport", "Gallofree Yards, Inc.", null, 90, 650, 6, 90, 19000000, "6 months", 4.0, 20),
            new Starship(21, "Slave 1", "Patrol craft", "Firespray-31-class patrol and attack", "Kuat Systems Engineering", null, 21.5, 1000, 1, 6, 70000, "1 month", 3.0, 70),
            new Starship(22, "Imperial shuttle", "Armed government transport", "Lambda-class T-4a shuttle", "Sienar Fleet Systems", 240000, 20, 850, 6, 20, 80000, "2 months", 1.0, 50),
            new Starship(23, "EF76 Nebulon-B escort frigate", "Escort ship", "EF76 Nebulon-B escort frigate", "Kuat Drive Yards", 8500000, 300, 800, 854, 75, 6000000, "2 years", 2.0, 40),
            new Starship(27, "Calamari Cruiser", "Star Cruiser", "MC80 Liberty type Star Cruiser", "Mon Calamari shipyards", 104000000, 1200, null, 5400, 1200, null, "2 years", 1.0, 60),
            new Starship(28, "A-wing", "Starfighter", "RZ-1 A-wing Interceptor", "Alliance Underground Engineering, Incom Corporation", 175000, 9.6, 1300, 1, 0, 40, "1 week", 1.0, 120),
            new Starship(29, "B-wing", "Assault Starfighter", "A/SF-01 B-wing starfighter", "Slayn & Korpil", 220000, 16.9, 950, 1, 0, 45, "1 week", 2.0, 91),
            new Starship(31, "Republic Cruiser", "Space cruiser", "Consular-class cruiser", "Corellian Engineering Corporation", null, 115, 900, 9, 16, null, "unknown", 2.0, null),
            new Starship(32, "Droid control ship", "Droid control ship", "Lucrehulk-class Droid Control Ship", "Hoersch-Kessel Drive, Inc.", null, 3170, null, 175, 139000, 4000000000, "500 days", 2.0, null),
            new Starship(39, "Naboo fighter", "Starfighter", "N-1 starfighter", "Theed Palace Space Vessel Engineering Corps", 200000, 11, 1100, 1, 0, 65, "7 days", 1.0, null),
            new Starship(40, "Naboo Royal Starship", "yacht", "J-type 327 Nubian royal starship", "Theed Palace Space Vessel Engineering Corps, Nubia Star Drives", null, 76, 920, 8, null, null, "unknown", 1.8, null),
            new Starship(41, "Scimitar", "Space Transport", "Star Courier", "Republic Sienar Systems", 55000000, 26.5, 1180, 1, 6, 2500000, "30 days", 1.5, null),
            new Starship(43, "J-type diplomatic barge", "Diplomatic barge", "J-type diplomatic barge", "Theed Palace Space Vessel Engineering Corps, Nubia Star Drives", 2000000, 39, 2000, 5, 10, null, "1 year", 0.7, null),
            new Starship(47, "AA-9 Coruscant freighter", "freighter", "Botajef AA-9 Freighter-Liner", "Botajef Shipyards", null, 390, null, null, 30000, null, "unknown", null, null),
            new Starship(48, "Jedi starfighter", "Starfighter", "Delta-7 Aethersprite-class interceptor", "Kuat Systems Engineering", 180000, 8, 1150, 1, 0, 60, "7 days", 1.0, null),
            new Starship(49, "H-type Nubian yacht", "yacht", "H-type Nubian yacht", "Theed Palace Space Vessel Engineering Corps", null, 47.9, 8000, 4, null, null, "unknown", 0.9, null),
            new Starship(52, "Republic Assault ship", "assault ship", "Acclamator I-class assault ship", "Rothana Heavy Engineering", null, 752, null, 700, 16000, 11250000, "2 years", 0.6, null),
            new Starship(58, "Solar Sailer", "yacht", "Punworcca 116-class interstellar sloop", "Huppla Pasa Tisc Shipwrights Collective", 35700, 15.2, 1600, 3, 11, 240, "7 days", 1.5, null),
            new Starship(59, "Trade Federation cruiser", "capital ship", "Providence-class carrier/destroyer", "Rendili StarDrive, Free Dac Volunteers Engineering corps.", 125000000, 1088, 1050, 600, 48247, 50000000, "4 years", 1.5, null),
            new Starship(61, "Theta-class T-2c shuttle", "transport", "Theta-class T-2c shuttle", "Cygnus Spaceworks", 1000000, 18.5, 2000, 5, 16, 50000, "56 days", 1.0, null),
            new Starship(63, "Republic attack cruiser", "star destroyer", "Senator-class Star Destroyer", "Kuat Drive Yards, Allanteen Six shipyards", 59000000, 1137, 975, 7400, 2000, 20000000, "2 years", 1.0, null),
            new Starship(64, "Naboo star skiff", "yacht", "J-type star skiff", "Theed Palace Space Vessel Engineering Corps/Nubia Star Drives, Incorporated", null, 29.2, 1050, 3, 3, null, "unknown", 0.5, null),
            new Starship(65, "Jedi Interceptor", "starfighter", "Eta-2 Actis-class light interceptor", "Kuat Systems Engineering", 320000, 5.47, 1500, 1, 0, 60, "2 days", 1.0, null),
            new Starship(66, "arc-170", "starfighter", "Aggressive Reconnaissance-170 starfighte", "Incom Corporation, Subpro Corporation", 155000, 14.5, 1000, 3, 0, 110, "5 days", 1.0, 100),
            new Starship(68, "Banking clan frigte", "cruiser", "Munificent-class star frigate", "Hoersch-Kessel Drive, Inc, Gwori Revolutionary Industries", 57000000, 825, null, 200, null, 40000000, "2 years", 1.0, null),
            new Starship(74, "Belbullab-22 starfighter", "starfighter", "Belbullab-22 starfighter", "Feethan Ottraw Scalable Assemblies", 168000, 6.71, 1100, 1, 0, 140, "7 days", 6, null),
            new Starship(75, "V-wing", "starfighter", "Alpha-3 Nimbus-class V-wing starfighter", "Kuat Systems Engineering", 102500, 7.9, 1050, 1, 0, 60, "15 hours", 1.0, null)
        };

    /// <summary>
    /// The list of vehicle entities.
    /// </summary>
    private static readonly List<Vehicle> vehiclesList = new()
        {
            new Vehicle(4, "Sand Crawler", "Digger Crawler", "wheeled", "Corellia Mining Corporation", 150000, 36.8, 46, 30, 30, 50000, "2 months"),
            new Vehicle(6, "T-16 skyhopper", "T-16 skyhopper", "repulsorcraft", "Incom Corporation", 14500, 10.4, 1, 1, 1200, 50, "0"),
            new Vehicle(7, "X-34 landspeeder", "X-34 landspeeder", "repulsorcraft", "SoroSuub Corporation", 10550, 3.4, 1, 1, 250, 5, "unknown"),
            new Vehicle(8, "TIE/LN starfighter", "Twin Ion Engine/Ln Starfighter", "starfighter", "Sienar Fleet Systems", null, 6.4, 1, 0, 1200, 65, "2 days"),
            new Vehicle(14, "Snowspeeder", "t-47 airspeeder", "airspeeder", "Incom corporation", null, 4.5, 2, 0, 650, 10, "none"),
            new Vehicle(16, "TIE bomber", "TIE/sa bomber", "space/planetary bomber", "Sienar Fleet Systems", null, 7.8, 1, 0, 850, 0, "2 days"),
            new Vehicle(18, "AT-AT", "All Terrain Armored Transport", "assault walker", "Kuat Drive Yards, Imperial Department of Military Research", null, 20, 5, 40, 60, 1000, "unknown"),
            new Vehicle(19, "AT-ST", "All Terrain Scout Transport", "walker", "Kuat Drive Yards, Imperial Department of Military Research", null, 2, 2, 0, 90, 200, "none"),
            new Vehicle(20, "Storm IV Twin-Pod cloud car", "Storm IV Twin-Pod", "repulsorcraft", "Bespin Motors", 75000, 7, 2, 0, 1500, 10, "1 day"),
            new Vehicle(24, "Sail barge", "Modified Luxury Sail Barge", "sail barge", "Ubrikkian Industries Custom Vehicle Division", 285000, 30, 26, 500, 100, 2000000, "Live food tanks"),
            new Vehicle(25, "Bantha-II cargo skiff", "Bantha-II", "repulsorcraft cargo skiff", "Ubrikkian Industries", 8000, 9.5, 5, 16, 250, 135000, "1 day"),
            new Vehicle(26, "TIE/IN interceptor", "Twin Ion Engine Interceptor", "starfighter", "Sienar Fleet Systems", null, 9.6, 1, 0, 1250, 75, "2 days"),
            new Vehicle(30, "Imperial Speeder Bike", "74-Z speeder bike", "speeder", "Aratech Repulsor Company", 8000, 3, 1, 1, 360, 4, "1 day"),
            new Vehicle(33, "Vulture Droid", "Vulture-class droid starfighter", "starfighter", "Haor Chall Engineering, Baktoid Armor Workshop", null, 3.5, 0, 0, 1200, 0, "none"),
            new Vehicle(34, "Multi-Troop Transport", "Multi-Troop Transport", "repulsorcraft", "Baktoid Armor Workshop", 138000, 31, 4, 112, 35, 12000, "unknown"),
            new Vehicle(35, "Armored Assault Tank", "Armoured Assault Tank", "repulsorcraft", "Baktoid Armor Workshop", null, 9.75, 4, 6, 55, null, "unknown"),
            new Vehicle(36, "Single Trooper Aerial Platform", "Single Trooper Aerial Platform", "repulsorcraft", "Baktoid Armor Workshop", 2500, 2, 1, 0, 400, 0, "none"),
            new Vehicle(37, "C-9979 landing craft", "C-9979 landing craft", "landing craft", "Haor Chall Engineering", 200000, 210, 140, 284, 587, 1800000, "1 day"),
            new Vehicle(38, "Tribubble bongo", "Tribubble bongo", "submarine", "Otoh Gunga Bongameken Cooperative", null, 15, 1, 2, 85, 1600, "unknown"),
            new Vehicle(42, "Sith speeder", "FC-20 speeder bike", "speeder", "Razalon", 4000, 1.5, 1, 0, 180, 2, "unknown"),
            new Vehicle(44, "Zephyr-G swoop bike", "Zephyr-G swoop bike", "repulsorcraft", "Mobquet Swoops and Speeders", 5750, 3.68, 1, 1, 350, 200, "none"),
            new Vehicle(45, "Koro-2 Exodrive airspeeder", "Koro-2 Exodrive airspeeder", "airspeeder", "Desler Gizh Outworld Mobility Corporation", null, 6.6, 1, 1, 800, 80, "unknown"),
            new Vehicle(46, "XJ-6 airspeeder", "XJ-6 airspeeder", "airspeeder", "Narglatch AirTech prefabricated kit", null, 6.23, 1, 1, 720, null, "unknown"),
            new Vehicle(50, "LAAT/i", "Low Altitude Assault Transport/infrantry", "gunship", "Rothana Heavy Engineering", null, 17.4, 6, 30, 620, 170, "unknown"),
            new Vehicle(51, "LAAT/c", "Low Altitude Assault Transport/carrier", "gunship", "Rothana Heavy Engineering", null, 28.82, 1, 0, 620, 40000, "unknown"),
            new Vehicle(53, "AT-TE", "All Terrain Tactical Enforcer", "walker", "Rothana Heavy Engineering, Kuat Drive Yards", null, 13.2, 6, 36, 60, 10000, "21 days"),
            new Vehicle(54, "SPHA", "Self-Propelled Heavy Artillery", "walker", "Rothana Heavy Engineering", null, 140, 25, 30, 35, 500, "7 days"),
            new Vehicle(55, "Flitknot speeder", "Flitknot speeder", "speeder", "Huppla Pasa Tisc Shipwrights Collective", 8000, 2, 1, 0, 634, null, "unknown"),
            new Vehicle(56, "Neimoidian shuttle", "Sheathipede-class transport shuttle", "transport", "Haor Chall Engineering", null, 20, 2, 6, 880, 1000, "7 days"),
            new Vehicle(57, "Geonosian starfighter", "Nantex-class territorial defense", "starfighter", "Huppla Pasa Tisc Shipwrights Collective", null, 9.8, 1, 0, 20000, null, "unknown"),
            new Vehicle(60, "Tsmeu-6 personal wheel bike", "Tsmeu-6 personal wheel bike", "wheeled walker", "Z-Gomot Ternbuell Guppat Corporation", 15000, 3.5, 1, 1, 330, 10, "none"),
            new Vehicle(62, "Emergency Firespeeder", "Fire suppression speeder", "fire suppression ship", "unknown", null, null, 2, null, null, null, "unknown"),
            new Vehicle(67, "Droid tri-fighter", "tri-fighter", "droid starfighter", "Colla Designs, Phlac-Arphocc Automata Industries", 20000, 5.4, 1, 0, 1180, 0, "none"),
            new Vehicle(69, "Oevvaor jet catamaran", "Oevvaor jet catamaran", "airspeeder", "Appazanna Engineering Works", 12125, 15.1, 2, 2, 420, 50, "3 days"),
            new Vehicle(70, "Raddaugh Gnasp fluttercraft", "Raddaugh Gnasp fluttercraft", "air speeder", "Appazanna Engineering Works", 14750, 7, 2, 0, 310, 20, "none"),
            new Vehicle(71, "Clone turbo tank", "HAVw A6 Juggernaut", "wheeled walker", "Kuat Drive Yards", 350000, 49.4, 20, 300, 160, 30000, "20 days"),
            new Vehicle(72, "Corporate Alliance tank droid", "NR-N99 Persuader-class droid enforcer", "droid tank", "Techno Union", 49000, 10.96, 0, 4, 100, 0, "none"),
            new Vehicle(73, "Droid gunship", "HMP droid gunship", "airspeeder", "Baktoid Fleet Ordnance, Haor Chall Engineering", 60000, 12.3, 0, 0, 820, 0, "none"),
            new Vehicle(76, "AT-RT", "All Terrain Recon Transport", "walker", "Kuat Drive Yards", 40000, 3.2, 1, 0, 90, 20, "1 day")
        };
    #endregion

    #region Entity Links
    private static int[] CharactersInFilm(int episodeId) => episodeId switch
    {
        1 => new int[] { 2, 3, 10, 11, 16, 20, 21, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 },
        2 => new int[] { 2, 3, 6, 7, 10, 11, 20, 21, 22, 33, 35, 36, 40, 43, 46, 51, 52, 53, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 82 },
        3 => new int[] { 1, 2, 3, 4, 5, 6, 7, 10, 11, 12, 13, 20, 21, 33, 35, 46, 51, 52, 53, 54, 55, 56, 58, 63, 64, 67, 68, 75, 78, 79, 80, 81, 82, 83 },
        4 => new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13, 14, 15, 16, 18, 19, 81 },
        5 => new int[] { 1, 2, 3, 4, 5, 10, 13, 14, 18, 20, 21, 22, 23, 24, 25, 26 },
        6 => new int[] { 1, 2, 3, 4, 5, 10, 13, 14, 16, 18, 20, 21, 22, 25, 27, 28, 29, 30, 31, 45 },
        _ => Array.Empty<int>()
    };

    private static int[] PlanetsInFilm(int episodeId) => episodeId switch
    {
        1 => new int[] { 1, 8, 9 },
        2 => new int[] { 1, 8, 9, 10, 11 },
        3 => new int[] { 1, 2, 5, 8, 9, 12, 13, 14, 15, 16, 17, 18, 19 },
        4 => new int[] { 1, 2, 3 },
        5 => new int[] { 4, 5, 6, 27 },
        6 => new int[] { 1, 5, 7, 8, 9 },
        _ => Array.Empty<int>()
    };

    private static int[] SpeciesInFilm(int episodeId) => episodeId switch
    {
        1 => new int[] { 1, 2, 6, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 },
        2 => new int[] { 1, 2, 6, 12, 13, 15, 28, 29, 30, 31, 32, 33, 34, 35 },
        3 => new int[] { 1, 2, 3, 6, 15, 19, 20, 23, 24, 25, 26, 27, 28, 29, 30, 33, 34, 35, 36, 37 },
        4 => new int[] { 1, 2, 3, 4, 5 },
        5 => new int[] { 1, 2, 3, 6, 7 },
        6 => new int[] { 1, 2, 3, 5, 6, 8, 9, 10, 15 },
        _ => Array.Empty<int>()
    };

    private static int[] StarshipsInFilm(int episodeId) => episodeId switch
    {
        1 => new int[] { 31, 32, 39, 40, 41 },
        2 => new int[] { 21, 32, 39, 43, 47, 48, 49, 52, 58 },
        3 => new int[] { 2, 32, 48, 59, 61, 63, 64, 65, 66, 68, 74, 75 },
        4 => new int[] { 2, 3, 5, 9, 10, 11, 12, 13 },
        5 => new int[] { 3, 10, 11, 12, 15, 17, 21, 22, 23 },
        6 => new int[] { 2, 3, 10, 11, 12, 15, 17, 22, 23, 27, 28, 29 },
        _ => Array.Empty<int>()
    };

    private static int[] VehiclesInFilm(int episodeId) => episodeId switch
    {
        1 => new int[] { 33, 34, 35, 36, 37, 38, 42 },
        2 => new int[] { 4, 44, 45, 46, 50, 51, 53, 54, 55, 56, 57 },
        3 => new int[] { 33, 50, 54, 56, 60, 62, 67, 69, 70, 71, 72, 73, 76 },
        4 => new int[] { 4, 6, 7, 8 },
        5 => new int[] { 8, 14, 16, 18, 19, 20 },
        6 => new int[] { 8, 16, 18, 19, 24, 25, 26, 30 },
        _ => Array.Empty<int>()
    };

    private static int? HomeworldForPerson(int personId) => personId switch
    {
        1 => 1,
        2 => 1,
        3 => 8,
        4 => 1,
        5 => 2,
        6 => 1,
        7 => 1,
        8 => 1,
        9 => 1,
        10 => 20,
        11 => 1,
        12 => 21,
        13 => 14,
        14 => 22,
        15 => 23,
        16 => 24,
        18 => 22,
        19 => 26,
        20 => 28,
        21 => 8,
        22 => 10,
        23 => 28,
        24 => 29,
        25 => 30,
        26 => 6,
        27 => 31,
        28 => 32,
        29 => 28,
        30 => 7,
        31 => 33,
        32 => 28,
        33 => 18,
        34 => 9,
        35 => 8,
        36 => 8,
        37 => 8,
        38 => 8,
        39 => 8,
        40 => 34,
        41 => 35,
        42 => 8,
        43 => 1,
        44 => 36,
        45 => 37,
        46 => 37,
        47 => 38,
        48 => 39,
        49 => 40,
        50 => 41,
        51 => 42,
        52 => 43,
        53 => 44,
        54 => 45,
        55 => 9,
        56 => 47,
        57 => 48,
        58 => 49,
        59 => 50,
        60 => 8,
        61 => 8,
        62 => 1,
        63 => 11,
        64 => 51,
        65 => 51,
        66 => 8,
        67 => 52,
        68 => 2,
        69 => 53,
        70 => 54,
        71 => 55,
        72 => 10,
        73 => 10,
        74 => 9,
        75 => 28,
        76 => 56,
        77 => 57,
        78 => 58,
        79 => 59,
        80 => 14,
        81 => 2,
        82 => 60,
        83 => 12,
        _ => null
    };

    private static int? HomeworldForSpecies(int speciesId) => speciesId switch
    {
        1 => 9,
        3 => 14,
        4 => 23,
        5 => 24,
        6 => 28,
        7 => 29,
        8 => 31,
        9 => 7,
        10 => 33,
        11 => 18,
        12 => 8,
        13 => 34,
        14 => 35,
        15 => 37,
        16 => 38,
        17 => 39,
        18 => 40,
        19 => 41,
        20 => 43,
        21 => 44,
        22 => 45,
        23 => 46,
        24 => 47,
        25 => 48,
        26 => 49,
        27 => 50,
        28 => 11,
        29 => 51,
        30 => 54,
        31 => 55,
        32 => 10,
        33 => 56,
        34 => 57,
        35 => 58,
        36 => 59,
        37 => 12,
        _ => null
    };

    private static int[] PeopleInSpecies(int speciesId) => speciesId switch
    {
        1 => new int[] { 1, 4, 5, 6, 7, 9, 10, 11, 12, 14, 18, 19, 21, 22, 25, 26, 28, 29, 32, 34, 35, 39, 42, 43, 51, 60, 61, 62, 66, 67, 68, 69, 74, 81, 82 },
        2 => new int[] { 2, 3, 8, 23 },
        3 => new int[] { 13, 80 },
        4 => new int[] { 15 },
        5 => new int[] { 16 },
        6 => new int[] { 20 },
        7 => new int[] { 24 },
        8 => new int[] { 27 },
        9 => new int[] { 30 },
        10 => new int[] { 31 },
        11 => new int[] { 33 },
        12 => new int[] { 36, 37, 38 },
        13 => new int[] { 40 },
        14 => new int[] { 41 },
        15 => new int[] { 45, 46 },
        16 => new int[] { 47 },
        17 => new int[] { 48 },
        18 => new int[] { 49 },
        19 => new int[] { 50 },
        20 => new int[] { 52 },
        21 => new int[] { 53 },
        22 => new int[] { 44, 54 },
        23 => new int[] { 55 },
        24 => new int[] { 56 },
        25 => new int[] { 57 },
        26 => new int[] { 58 },
        27 => new int[] { 59 },
        28 => new int[] { 63 },
        29 => new int[] { 64, 65 },
        30 => new int[] { 70 },
        31 => new int[] { 71 },
        32 => new int[] { 72, 73 },
        33 => new int[] { 76 },
        34 => new int[] { 77 },
        35 => new int[] { 78 },
        36 => new int[] { 79 },
        37 => new int[] { 83 },
        _ => Array.Empty<int>()
    };

    private static int[] PilotsForStarship(int starshipId) => starshipId switch
    {
        10 => new int[] { 13, 14, 25, 31 },
        12 => new int[] { 1, 9, 18, 19 },
        13 => new int[] { 4 },
        21 => new int[] { 22 },
        22 => new int[] { 1, 13, 14 },
        28 => new int[] { 29 },
        39 => new int[] { 11, 35, 60 },
        40 => new int[] { 39 },
        41 => new int[] { 44 },
        48 => new int[] { 10, 58 },
        49 => new int[] { 35 },
        59 => new int[] { 10, 11 },
        64 => new int[] { 10, 35 },
        65 => new int[] { 10, 11 },
        74 => new int[] { 10, 79 },
        _ => Array.Empty<int>()
    };

    private static int[] PilotsForVehicle(int id) => id switch
    {
        14 => new int[] { 1, 18 },
        19 => new int[] { 13 },
        30 => new int[] { 1, 5 },
        38 => new int[] { 10, 32 },
        42 => new int[] { 44 },
        44 => new int[] { 11 },
        45 => new int[] { 70 },
        46 => new int[] { 11 },
        55 => new int[] { 67 },
        60 => new int[] { 79 },
        _ => Array.Empty<int>()
    };
    #endregion
}
