using System;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp.Data
{
    public enum Biome
    {
        the_void = 0,
        plains = 1,
        sunflower_plains = 2,
        snowy_plains = 3,
        ice_spikes = 4,
        desert = 5,
        swamp = 6,
        mangrove_swamp = 7,
        forest = 8,
        flower_forest = 9,
        birch_forest = 10,
        dark_forest = 11,
        old_growth_birch_forest = 12,
        old_growth_pine_taiga = 13,
        old_growth_spruce_taiga = 14,
        taiga = 15,
        snowy_taiga = 16,
        savanna = 17,
        savanna_plateau = 18,
        windswept_hills = 19,
        windswept_gravelly_hills = 20,
        windswept_forest = 21,
        windswept_savanna = 22,
        jungle = 23,
        sparse_jungle = 24,
        bamboo_jungle = 25,
        badlands = 26,
        eroded_badlands = 27,
        wooded_badlands = 28,
        meadow = 29,
        grove = 30,
        snowy_slopes = 31,
        frozen_peaks = 32,
        jagged_peaks = 33,
        stony_peaks = 34,
        river = 35,
        frozen_river = 36,
        beach = 37,
        snowy_beach = 38,
        stony_shore = 39,
        warm_ocean = 40,
        lukewarm_ocean = 41,
        deep_lukewarm_ocean = 42,
        ocean = 43,
        deep_ocean = 44,
        cold_ocean = 45,
        deep_cold_ocean = 46,
        frozen_ocean = 47,
        deep_frozen_ocean = 48,
        mushroom_fields = 49,
        dripstone_caves = 50,
        lush_caves = 51,
        deep_dark = 52,
        nether_wastes = 53,
        warped_forest = 54,
        crimson_forest = 55,
        soul_sand_valley = 56,
        basalt_deltas = 57,
        the_end = 58,
        end_highlands = 59,
        end_midlands = 60,
        small_end_islands = 61,
        end_barrens = 62,
        UNKNOWN = 999
    }

    /// <summary>
    /// Categories for biomes. Description and information as per https://minecraft.fandom.com/wiki/Biome#Overworld
    /// </summary>
    public static class BiomeCategories
    {
        /// <summary>
        /// In these biomes, it always snows instead of rains and no matter the height; all sources of water exposed to the sky quickly freeze.
        /// The foliage and grass have a dull aqua green color.
        /// </summary>
        public static Biome[] Snowy
        {
            get
            {
                return new Biome[]
                {
                    Biome.snowy_plains,
                    Biome.ice_spikes,
                    Biome.snowy_taiga,
                    Biome.snowy_beach,
                    Biome.grove,
                    Biome.snowy_slopes,
                    Biome.jagged_peaks,
                    Biome.frozen_peaks
                };
            }
        }

        /// <summary>
        /// In these biomes, it begins to snow above y=120 in windswept hills and stony shore, above y=160 in taiga and old growth spruce taiga,
        /// and above y=200 in old growth pine taiga. Otherwise, it rains. Foliage and grass are turquoise green in these biomes.
        /// </summary>
        public static Biome[] Cold
        {
            get
            {
                return new Biome[]
                {
                    Biome.windswept_hills,
                    Biome.windswept_gravelly_hills,
                    Biome.windswept_forest,
                    Biome.taiga,
                    Biome.old_growth_pine_taiga,
                    Biome.old_growth_spruce_taiga,
                    Biome.stony_shore
                };
            }
        }

        public static Biome[] Temperate
        {
            get
            {
                return new Biome[]
                {
                    Biome.plains,
                    Biome.sunflower_plains,
                    Biome.forest,
                    Biome.flower_forest,
                    Biome.birch_forest,
                    Biome.old_growth_birch_forest,
                    Biome.dark_forest,
                    Biome.swamp,
                    Biome.mangrove_swamp,
                    Biome.jungle,
                    Biome.sparse_jungle,
                    Biome.bamboo_jungle,
                    Biome.beach,
                    Biome.mushroom_fields,
                    Biome.meadow,
                    Biome.stony_peaks
                };
            }
        }

        /// <summary>
        /// In these biomes, it neither rains nor snows at all, but the sky still turns overcast during inclement weather.
        /// The foliage and grass is an olive tone, except badlands biomes, which have brown grass and the sky color is much brighter specially in deserts
        /// and badlands.
        /// Additionally, a snow golem spawned or brought into one of these biomes melts (takes heat damage) unless it has the Fire Resistance effect.
        /// </summary>
        public static Biome[] Warm
        {
            get
            {
                return new Biome[]
                {
                    Biome.desert,
                    Biome.savanna,
                    Biome.savanna_plateau,
                    Biome.windswept_savanna,
                    Biome.badlands,
                    Biome.wooded_badlands,
                    Biome.eroded_badlands
                };
            }
        }

        /// <summary>
        /// These biomes are used for the generation of bodies of water such as rivers and oceans.
        /// Oceans are large, open biomes made entirely of water going up to y=63, with underwater relief on the sea floor, such as small mountains and plains,
        /// usually including gravel. Squid and fish spawn frequently in the water.
        /// Underwater cave entrances can be found frequently at the bottom of oceans and rivers. These are the only biomes where underwater music plays.
        /// </summary>
        public static Biome[] Aquatic
        {
            get
            {
                return new Biome[]
                {
                    Biome.river,
                    Biome.frozen_river,
                    Biome.warm_ocean,
                    Biome.lukewarm_ocean,
                    Biome.deep_lukewarm_ocean,
                    Biome.ocean,
                    Biome.deep_ocean,
                    Biome.cold_ocean,
                    Biome.deep_cold_ocean,
                    Biome.frozen_ocean,
                    Biome.deep_frozen_ocean
                };
            }
        }

        public static Biome[] ColdAquatic
        {
            get
            {
                return new Biome[]
                {
                    Biome.frozen_river,
                    Biome.cold_ocean,
                    Biome.deep_cold_ocean,
                    Biome.frozen_ocean,
                    Biome.deep_frozen_ocean
                };
            }
        }

        public static Biome[] TemperateAquatic
        {
            get
            {
                return new Biome[]
                {
                    Biome.river,
                    Biome.lukewarm_ocean,
                    Biome.deep_lukewarm_ocean,
                    Biome.ocean,
                    Biome.deep_ocean
                };
            }
        }

        public static Biome[] WarmAquatic
        {
            get
            {
                return new Biome[]
                {
                    Biome.warm_ocean
                };
            }
        }

        /// <summary>
        /// These biomes generates inside caves. Their placement are 3D, compared to other Overworld biomes, which use 2D.
        /// They're mostly found underground but can sometimes leak out of cave entrances at any height.
        /// </summary>
        public static Biome[] Cave
        {
            get
            {
                return new Biome[]
                {
                    Biome.deep_dark,
                    Biome.dripstone_caves,
                    Biome.lush_caves
                };
            }
        }

        /// <summary>
        /// The Nether is considered a different dimension.
        /// It is a hellish place; all biomes in this dimension are dry and it is not possible to place water in these biomes, though ice can still be placed.
        /// Additionally, packed ice and blue ice never melt in the nether, as with the other non-freezing biomes.
        /// </summary>
        public static Biome[] Nether
        {
            get
            {
                return new Biome[]
                {
                    Biome.nether_wastes,
                    Biome.soul_sand_valley,
                    Biome.crimson_forest,
                    Biome.warped_forest,
                    Biome.basalt_deltas
                };
            }
        }

        public static Biome[] End
        {
            get
            {
                return new Biome[]
                {
                    Biome.the_end,
                    Biome.small_end_islands,
                    Biome.end_midlands,
                    Biome.end_highlands,
                    Biome.end_barrens
                };
            }
        }
    }
}
