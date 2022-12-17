﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GDMCHttp.Data
{
    public enum BlockName
    {
        air,
        stone,
        granite,
        polished_granite,
        diorite,
        polished_diorite,
        andesite,
        polished_andesite,
        deepslate,
        cobbled_deepslate,
        polished_deepslate,
        calcite,
        tuff,
        dripstone_block,
        grass_block,
        dirt,
        coarse_dirt,
        podzol,
        rooted_dirt,
        mud,
        crimson_nylium,
        warped_nylium,
        cobblestone,
        oak_planks,
        spruce_planks,
        birch_planks,
        jungle_planks,
        acacia_planks,
        dark_oak_planks,
        mangrove_planks,
        crimson_planks,
        warped_planks,
        oak_sapling,
        spruce_sapling,
        birch_sapling,
        jungle_sapling,
        acacia_sapling,
        dark_oak_sapling,
        mangrove_propagule,
        bedrock,
        sand,
        red_sand,
        gravel,
        coal_ore,
        deepslate_coal_ore,
        iron_ore,
        deepslate_iron_ore,
        copper_ore,
        deepslate_copper_ore,
        gold_ore,
        deepslate_gold_ore,
        redstone_ore,
        deepslate_redstone_ore,
        emerald_ore,
        deepslate_emerald_ore,
        lapis_ore,
        deepslate_lapis_ore,
        diamond_ore,
        deepslate_diamond_ore,
        nether_gold_ore,
        nether_quartz_ore,
        ancient_debris,
        coal_block,
        raw_iron_block,
        raw_copper_block,
        raw_gold_block,
        amethyst_block,
        budding_amethyst,
        iron_block,
        copper_block,
        gold_block,
        diamond_block,
        netherite_block,
        exposed_copper,
        weathered_copper,
        oxidized_copper,
        cut_copper,
        exposed_cut_copper,
        weathered_cut_copper,
        oxidized_cut_copper,
        cut_copper_stairs,
        exposed_cut_copper_stairs,
        weathered_cut_copper_stairs,
        oxidized_cut_copper_stairs,
        cut_copper_slab,
        exposed_cut_copper_slab,
        weathered_cut_copper_slab,
        oxidized_cut_copper_slab,
        waxed_copper_block,
        waxed_exposed_copper,
        waxed_weathered_copper,
        waxed_oxidized_copper,
        waxed_cut_copper,
        waxed_exposed_cut_copper,
        waxed_weathered_cut_copper,
        waxed_oxidized_cut_copper,
        waxed_cut_copper_stairs,
        waxed_exposed_cut_copper_stairs,
        waxed_weathered_cut_copper_stairs,
        waxed_oxidized_cut_copper_stairs,
        waxed_cut_copper_slab,
        waxed_exposed_cut_copper_slab,
        waxed_weathered_cut_copper_slab,
        waxed_oxidized_cut_copper_slab,
        oak_log,
        spruce_log,
        birch_log,
        jungle_log,
        acacia_log,
        dark_oak_log,
        mangrove_log,
        mangrove_roots,
        muddy_mangrove_roots,
        crimson_stem,
        warped_stem,
        stripped_oak_log,
        stripped_spruce_log,
        stripped_birch_log,
        stripped_jungle_log,
        stripped_acacia_log,
        stripped_dark_oak_log,
        stripped_mangrove_log,
        stripped_crimson_stem,
        stripped_warped_stem,
        stripped_oak_wood,
        stripped_spruce_wood,
        stripped_birch_wood,
        stripped_jungle_wood,
        stripped_acacia_wood,
        stripped_dark_oak_wood,
        stripped_mangrove_wood,
        stripped_crimson_hyphae,
        stripped_warped_hyphae,
        oak_wood,
        spruce_wood,
        birch_wood,
        jungle_wood,
        acacia_wood,
        dark_oak_wood,
        mangrove_wood,
        crimson_hyphae,
        warped_hyphae,
        oak_leaves,
        spruce_leaves,
        birch_leaves,
        jungle_leaves,
        acacia_leaves,
        dark_oak_leaves,
        mangrove_leaves,
        azalea_leaves,
        flowering_azalea_leaves,
        sponge,
        wet_sponge,
        glass,
        tinted_glass,
        lapis_block,
        sandstone,
        chiseled_sandstone,
        cut_sandstone,
        cobweb,
        grass,
        fern,
        azalea,
        flowering_azalea,
        dead_bush,
        seagrass,
        sea_pickle,
        white_wool,
        orange_wool,
        magenta_wool,
        light_blue_wool,
        yellow_wool,
        lime_wool,
        pink_wool,
        gray_wool,
        light_gray_wool,
        cyan_wool,
        purple_wool,
        blue_wool,
        brown_wool,
        green_wool,
        red_wool,
        black_wool,
        dandelion,
        poppy,
        blue_orchid,
        allium,
        azure_bluet,
        red_tulip,
        orange_tulip,
        white_tulip,
        pink_tulip,
        oxeye_daisy,
        cornflower,
        lily_of_the_valley,
        wither_rose,
        spore_blossom,
        brown_mushroom,
        red_mushroom,
        crimson_fungus,
        warped_fungus,
        crimson_roots,
        warped_roots,
        nether_sprouts,
        weeping_vines,
        twisting_vines,
        sugar_cane,
        kelp,
        moss_carpet,
        moss_block,
        hanging_roots,
        big_dripleaf,
        small_dripleaf,
        bamboo,
        oak_slab,
        spruce_slab,
        birch_slab,
        jungle_slab,
        acacia_slab,
        dark_oak_slab,
        mangrove_slab,
        crimson_slab,
        warped_slab,
        stone_slab,
        smooth_stone_slab,
        sandstone_slab,
        cut_sandstone_slab,
        petrified_oak_slab,
        cobblestone_slab,
        brick_slab,
        stone_brick_slab,
        mud_brick_slab,
        nether_brick_slab,
        quartz_slab,
        red_sandstone_slab,
        cut_red_sandstone_slab,
        purpur_slab,
        prismarine_slab,
        prismarine_brick_slab,
        dark_prismarine_slab,
        smooth_quartz,
        smooth_red_sandstone,
        smooth_sandstone,
        smooth_stone,
        bricks,
        bookshelf,
        mossy_cobblestone,
        obsidian,
        torch,
        end_rod,
        chorus_plant,
        chorus_flower,
        purpur_block,
        purpur_pillar,
        purpur_stairs,
        spawner,
        chest,
        crafting_table,
        farmland,
        furnace,
        ladder,
        cobblestone_stairs,
        snow,
        ice,
        snow_block,
        cactus,
        clay,
        jukebox,
        oak_fence,
        spruce_fence,
        birch_fence,
        jungle_fence,
        acacia_fence,
        dark_oak_fence,
        mangrove_fence,
        crimson_fence,
        warped_fence,
        pumpkin,
        carved_pumpkin,
        jack_o_lantern,
        netherrack,
        soul_sand,
        soul_soil,
        basalt,
        polished_basalt,
        smooth_basalt,
        soul_torch,
        glowstone,
        infested_stone,
        infested_cobblestone,
        infested_stone_bricks,
        infested_mossy_stone_bricks,
        infested_cracked_stone_bricks,
        infested_chiseled_stone_bricks,
        infested_deepslate,
        stone_bricks,
        mossy_stone_bricks,
        cracked_stone_bricks,
        chiseled_stone_bricks,
        packed_mud,
        mud_bricks,
        deepslate_bricks,
        cracked_deepslate_bricks,
        deepslate_tiles,
        cracked_deepslate_tiles,
        chiseled_deepslate,
        reinforced_deepslate,
        brown_mushroom_block,
        red_mushroom_block,
        mushroom_stem,
        iron_bars,
        chain,
        glass_pane,
        melon,
        vine,
        glow_lichen,
        brick_stairs,
        stone_brick_stairs,
        mud_brick_stairs,
        mycelium,
        lily_pad,
        nether_bricks,
        cracked_nether_bricks,
        chiseled_nether_bricks,
        nether_brick_fence,
        nether_brick_stairs,
        sculk,
        sculk_vein,
        sculk_catalyst,
        sculk_shrieker,
        enchanting_table,
        end_portal_frame,
        end_stone,
        end_stone_bricks,
        dragon_egg,
        sandstone_stairs,
        ender_chest,
        emerald_block,
        oak_stairs,
        spruce_stairs,
        birch_stairs,
        jungle_stairs,
        acacia_stairs,
        dark_oak_stairs,
        mangrove_stairs,
        crimson_stairs,
        warped_stairs,
        command_block,
        beacon,
        cobblestone_wall,
        mossy_cobblestone_wall,
        brick_wall,
        prismarine_wall,
        red_sandstone_wall,
        mossy_stone_brick_wall,
        granite_wall,
        stone_brick_wall,
        mud_brick_wall,
        nether_brick_wall,
        andesite_wall,
        red_nether_brick_wall,
        sandstone_wall,
        end_stone_brick_wall,
        diorite_wall,
        blackstone_wall,
        polished_blackstone_wall,
        polished_blackstone_brick_wall,
        cobbled_deepslate_wall,
        polished_deepslate_wall,
        deepslate_brick_wall,
        deepslate_tile_wall,
        anvil,
        chipped_anvil,
        damaged_anvil,
        chiseled_quartz_block,
        quartz_block,
        quartz_bricks,
        quartz_pillar,
        quartz_stairs,
        white_terracotta,
        orange_terracotta,
        magenta_terracotta,
        light_blue_terracotta,
        yellow_terracotta,
        lime_terracotta,
        pink_terracotta,
        gray_terracotta,
        light_gray_terracotta,
        cyan_terracotta,
        purple_terracotta,
        blue_terracotta,
        brown_terracotta,
        green_terracotta,
        red_terracotta,
        black_terracotta,
        barrier,
        light,
        hay_block,
        white_carpet,
        orange_carpet,
        magenta_carpet,
        light_blue_carpet,
        yellow_carpet,
        lime_carpet,
        pink_carpet,
        gray_carpet,
        light_gray_carpet,
        cyan_carpet,
        purple_carpet,
        blue_carpet,
        brown_carpet,
        green_carpet,
        red_carpet,
        black_carpet,
        terracotta,
        packed_ice,
        dirt_path,
        sunflower,
        lilac,
        rose_bush,
        peony,
        tall_grass,
        large_fern,
        white_stained_glass,
        orange_stained_glass,
        magenta_stained_glass,
        light_blue_stained_glass,
        yellow_stained_glass,
        lime_stained_glass,
        pink_stained_glass,
        gray_stained_glass,
        light_gray_stained_glass,
        cyan_stained_glass,
        purple_stained_glass,
        blue_stained_glass,
        brown_stained_glass,
        green_stained_glass,
        red_stained_glass,
        black_stained_glass,
        white_stained_glass_pane,
        orange_stained_glass_pane,
        magenta_stained_glass_pane,
        light_blue_stained_glass_pane,
        yellow_stained_glass_pane,
        lime_stained_glass_pane,
        pink_stained_glass_pane,
        gray_stained_glass_pane,
        light_gray_stained_glass_pane,
        cyan_stained_glass_pane,
        purple_stained_glass_pane,
        blue_stained_glass_pane,
        brown_stained_glass_pane,
        green_stained_glass_pane,
        red_stained_glass_pane,
        black_stained_glass_pane,
        prismarine,
        prismarine_bricks,
        dark_prismarine,
        prismarine_stairs,
        prismarine_brick_stairs,
        dark_prismarine_stairs,
        sea_lantern,
        red_sandstone,
        chiseled_red_sandstone,
        cut_red_sandstone,
        red_sandstone_stairs,
        repeating_command_block,
        chain_command_block,
        magma_block,
        nether_wart_block,
        warped_wart_block,
        red_nether_bricks,
        bone_block,
        structure_void,
        shulker_box,
        white_shulker_box,
        orange_shulker_box,
        magenta_shulker_box,
        light_blue_shulker_box,
        yellow_shulker_box,
        lime_shulker_box,
        pink_shulker_box,
        gray_shulker_box,
        light_gray_shulker_box,
        cyan_shulker_box,
        purple_shulker_box,
        blue_shulker_box,
        brown_shulker_box,
        green_shulker_box,
        red_shulker_box,
        black_shulker_box,
        white_glazed_terracotta,
        orange_glazed_terracotta,
        magenta_glazed_terracotta,
        light_blue_glazed_terracotta,
        yellow_glazed_terracotta,
        lime_glazed_terracotta,
        pink_glazed_terracotta,
        gray_glazed_terracotta,
        light_gray_glazed_terracotta,
        cyan_glazed_terracotta,
        purple_glazed_terracotta,
        blue_glazed_terracotta,
        brown_glazed_terracotta,
        green_glazed_terracotta,
        red_glazed_terracotta,
        black_glazed_terracotta,
        white_concrete,
        orange_concrete,
        magenta_concrete,
        light_blue_concrete,
        yellow_concrete,
        lime_concrete,
        pink_concrete,
        gray_concrete,
        light_gray_concrete,
        cyan_concrete,
        purple_concrete,
        blue_concrete,
        brown_concrete,
        green_concrete,
        red_concrete,
        black_concrete,
        white_concrete_powder,
        orange_concrete_powder,
        magenta_concrete_powder,
        light_blue_concrete_powder,
        yellow_concrete_powder,
        lime_concrete_powder,
        pink_concrete_powder,
        gray_concrete_powder,
        light_gray_concrete_powder,
        cyan_concrete_powder,
        purple_concrete_powder,
        blue_concrete_powder,
        brown_concrete_powder,
        green_concrete_powder,
        red_concrete_powder,
        black_concrete_powder,
        turtle_egg,
        dead_tube_coral_block,
        dead_brain_coral_block,
        dead_bubble_coral_block,
        dead_fire_coral_block,
        dead_horn_coral_block,
        tube_coral_block,
        brain_coral_block,
        bubble_coral_block,
        fire_coral_block,
        horn_coral_block,
        tube_coral,
        brain_coral,
        bubble_coral,
        fire_coral,
        horn_coral,
        dead_brain_coral,
        dead_bubble_coral,
        dead_fire_coral,
        dead_horn_coral,
        dead_tube_coral,
        tube_coral_fan,
        brain_coral_fan,
        bubble_coral_fan,
        fire_coral_fan,
        horn_coral_fan,
        dead_tube_coral_fan,
        dead_brain_coral_fan,
        dead_bubble_coral_fan,
        dead_fire_coral_fan,
        dead_horn_coral_fan,
        blue_ice,
        conduit,
        polished_granite_stairs,
        smooth_red_sandstone_stairs,
        mossy_stone_brick_stairs,
        polished_diorite_stairs,
        mossy_cobblestone_stairs,
        end_stone_brick_stairs,
        stone_stairs,
        smooth_sandstone_stairs,
        smooth_quartz_stairs,
        granite_stairs,
        andesite_stairs,
        red_nether_brick_stairs,
        polished_andesite_stairs,
        diorite_stairs,
        cobbled_deepslate_stairs,
        polished_deepslate_stairs,
        deepslate_brick_stairs,
        deepslate_tile_stairs,
        polished_granite_slab,
        smooth_red_sandstone_slab,
        mossy_stone_brick_slab,
        polished_diorite_slab,
        mossy_cobblestone_slab,
        end_stone_brick_slab,
        smooth_sandstone_slab,
        smooth_quartz_slab,
        granite_slab,
        andesite_slab,
        red_nether_brick_slab,
        polished_andesite_slab,
        diorite_slab,
        cobbled_deepslate_slab,
        polished_deepslate_slab,
        deepslate_brick_slab,
        deepslate_tile_slab,
        scaffolding,
        redstone_torch,
        redstone_block,
        repeater,
        comparator,
        piston,
        sticky_piston,
        slime_block,
        honey_block,
        observer,
        hopper,
        dispenser,
        dropper,
        lectern,
        target,
        lever,
        lightning_rod,
        daylight_detector,
        sculk_sensor,
        tripwire_hook,
        trapped_chest,
        tnt,
        redstone_lamp,
        note_block,
        stone_button,
        polished_blackstone_button,
        oak_button,
        spruce_button,
        birch_button,
        jungle_button,
        acacia_button,
        dark_oak_button,
        mangrove_button,
        crimson_button,
        warped_button,
        stone_pressure_plate,
        polished_blackstone_pressure_plate,
        light_weighted_pressure_plate,
        heavy_weighted_pressure_plate,
        oak_pressure_plate,
        spruce_pressure_plate,
        birch_pressure_plate,
        jungle_pressure_plate,
        acacia_pressure_plate,
        dark_oak_pressure_plate,
        mangrove_pressure_plate,
        crimson_pressure_plate,
        warped_pressure_plate,
        iron_door,
        oak_door,
        spruce_door,
        birch_door,
        jungle_door,
        acacia_door,
        dark_oak_door,
        mangrove_door,
        crimson_door,
        warped_door,
        iron_trapdoor,
        oak_trapdoor,
        spruce_trapdoor,
        birch_trapdoor,
        jungle_trapdoor,
        acacia_trapdoor,
        dark_oak_trapdoor,
        mangrove_trapdoor,
        crimson_trapdoor,
        warped_trapdoor,
        oak_fence_gate,
        spruce_fence_gate,
        birch_fence_gate,
        jungle_fence_gate,
        acacia_fence_gate,
        dark_oak_fence_gate,
        mangrove_fence_gate,
        crimson_fence_gate,
        warped_fence_gate,
        powered_rail,
        detector_rail,
        rail,
        activator_rail,
        structure_block,
        jigsaw,
        wheat,
        oak_sign,
        spruce_sign,
        birch_sign,
        jungle_sign,
        acacia_sign,
        dark_oak_sign,
        mangrove_sign,
        crimson_sign,
        warped_sign,
        dried_kelp_block,
        cake,
        white_bed,
        orange_bed,
        magenta_bed,
        light_blue_bed,
        yellow_bed,
        lime_bed,
        pink_bed,
        gray_bed,
        light_gray_bed,
        cyan_bed,
        purple_bed,
        blue_bed,
        brown_bed,
        green_bed,
        red_bed,
        black_bed,
        nether_wart,
        brewing_stand,
        cauldron,
        flower_pot,
        skeleton_skull,
        wither_skeleton_skull,
        player_head,
        zombie_head,
        creeper_head,
        dragon_head,
        white_banner,
        orange_banner,
        magenta_banner,
        light_blue_banner,
        yellow_banner,
        lime_banner,
        pink_banner,
        gray_banner,
        light_gray_banner,
        cyan_banner,
        purple_banner,
        blue_banner,
        brown_banner,
        green_banner,
        red_banner,
        black_banner,
        loom,
        composter,
        barrel,
        smoker,
        blast_furnace,
        cartography_table,
        fletching_table,
        grindstone,
        smithing_table,
        stonecutter,
        bell,
        lantern,
        soul_lantern,
        campfire,
        soul_campfire,
        shroomlight,
        bee_nest,
        beehive,
        honeycomb_block,
        lodestone,
        crying_obsidian,
        blackstone,
        blackstone_slab,
        blackstone_stairs,
        gilded_blackstone,
        polished_blackstone,
        polished_blackstone_slab,
        polished_blackstone_stairs,
        chiseled_polished_blackstone,
        polished_blackstone_bricks,
        polished_blackstone_brick_slab,
        polished_blackstone_brick_stairs,
        cracked_polished_blackstone_bricks,
        respawn_anchor,
        candle,
        white_candle,
        orange_candle,
        magenta_candle,
        light_blue_candle,
        yellow_candle,
        lime_candle,
        pink_candle,
        gray_candle,
        light_gray_candle,
        cyan_candle,
        purple_candle,
        blue_candle,
        brown_candle,
        green_candle,
        red_candle,
        black_candle,
        small_amethyst_bud,
        medium_amethyst_bud,
        large_amethyst_bud,
        amethyst_cluster,
        pointed_dripstone,
        ochre_froglight,
        verdant_froglight,
        pearlescent_froglight,
        frogspawn,
        water,
        lava,
        tall_seagrass,
        piston_head,
        moving_piston,
        wall_torch,
        fire,
        redstone_wire,
        oak_wall_sign,
        spruce_wall_sign,
        birch_wall_sign,
        acacia_wall_sign,
        jungle_wall_sign,
        dark_oak_wall_sign,
        mangrove_wall_sign,
        redstone_wall_torch,
        soul_wall_torch,
        nether_portal,
        attached_pumpkin_stem,
        attached_melon_stem,
        pumpkin_stem,
        melon_stem,
        water_cauldron,
        lava_cauldron,
        powder_snow_cauldron,
        end_portal,
        cocoa,
        tripwire,
        potted_oak_sapling,
        potted_spruce_sapling,
        potted_birch_sapling,
        potted_jungle_sapling,
        potted_acacia_sapling,
        potted_dark_oak_sapling,
        potted_mangrove_propagule,
        potted_fern,
        potted_dandelion,
        potted_poppy,
        potted_blue_orchid,
        potted_allium,
        potted_azure_bluet,
        potted_red_tulip,
        potted_orange_tulip,
        potted_white_tulip,
        potted_pink_tulip,
        potted_oxeye_daisy,
        potted_cornflower,
        potted_lily_of_the_valley,
        potted_wither_rose,
        potted_red_mushroom,
        potted_brown_mushroom,
        potted_dead_bush,
        potted_cactus,
        carrots,
        potatoes,
        skeleton_wall_skull,
        wither_skeleton_wall_skull,
        zombie_wall_head,
        player_wall_head,
        creeper_wall_head,
        dragon_wall_head,
        white_wall_banner,
        orange_wall_banner,
        magenta_wall_banner,
        light_blue_wall_banner,
        yellow_wall_banner,
        lime_wall_banner,
        pink_wall_banner,
        gray_wall_banner,
        light_gray_wall_banner,
        cyan_wall_banner,
        purple_wall_banner,
        blue_wall_banner,
        brown_wall_banner,
        green_wall_banner,
        red_wall_banner,
        black_wall_banner,
        beetroots,
        end_gateway,
        frosted_ice,
        kelp_plant,
        dead_tube_coral_wall_fan,
        dead_brain_coral_wall_fan,
        dead_bubble_coral_wall_fan,
        dead_fire_coral_wall_fan,
        dead_horn_coral_wall_fan,
        tube_coral_wall_fan,
        brain_coral_wall_fan,
        bubble_coral_wall_fan,
        fire_coral_wall_fan,
        horn_coral_wall_fan,
        bamboo_sapling,
        potted_bamboo,
        void_air,
        cave_air,
        bubble_column,
        sweet_berry_bush,
        weeping_vines_plant,
        twisting_vines_plant,
        crimson_wall_sign,
        warped_wall_sign,
        potted_crimson_fungus,
        potted_warped_fungus,
        potted_crimson_roots,
        potted_warped_roots,
        candle_cake,
        white_candle_cake,
        orange_candle_cake,
        magenta_candle_cake,
        light_blue_candle_cake,
        yellow_candle_cake,
        lime_candle_cake,
        pink_candle_cake,
        gray_candle_cake,
        light_gray_candle_cake,
        cyan_candle_cake,
        purple_candle_cake,
        blue_candle_cake,
        brown_candle_cake,
        green_candle_cake,
        red_candle_cake,
        black_candle_cake,
        powder_snow,
        cave_vines,
        cave_vines_plant,
        big_dripleaf_stem,
        potted_azalea_bush,
        potted_flowering_azalea_bush,
        UNKNOWN
    }

    public static class BlockCategories
    {
        private static HashSet<BlockName> groundBlocks;
        private static BlockName[] allBlockNames;
        private static HashSet<BlockName> plantBlocks;

        private static HashSet<BlockName> GroundBlocks
        {
            get
            {
                if (groundBlocks == null) groundBlocks = GenerateGroundBlocks();
                return groundBlocks;
            }
            set { groundBlocks = value; }
        }


        private static HashSet<BlockName> PlantBlocks
        {
            get
            {
                if (plantBlocks == null) plantBlocks = GeneratePlantBlocks();
                return plantBlocks;
            }
            set { plantBlocks = value; }
        }

        private static BlockName[] AllBlockNames
        {
            get
            {
                if (allBlockNames == null) allBlockNames = Enum.GetValues(typeof(BlockName)).Cast<BlockName>().ToArray();
                return allBlockNames;
            }
            set { allBlockNames = value; }
        }

        private static HashSet<BlockName> GeneratePlantBlocks()
        {
            HashSet<BlockName> plantBlocks = new HashSet<BlockName>()
        {
            BlockName.allium,
            BlockName.cactus,
            BlockName.wheat,
            BlockName.potatoes,
            BlockName.carrots,
            BlockName.oxeye_daisy,
            BlockName.dandelion
        };


            // Add groups, eg all woods
            string[] keyWords = new string[]
            {
            "log",
            "leaves",
            "sapling",
            "tulip",
            "orchid",
            "grass",
            "fern"
            };

            foreach (BlockName blockName in AllBlockNames)
            {
                for (int i = 0; i < keyWords.Length; i++)
                {
                    if (blockName.ToString().Contains(keyWords[i]))
                    {
                        groundBlocks.Add(blockName);
                        break;
                    }
                }

            }

            return plantBlocks;
        }

        private static HashSet<BlockName> GenerateGroundBlocks()
        {

            HashSet<BlockName> groundBlocks = new HashSet<BlockName>()
        {
            BlockName.dirt,
            BlockName.coarse_dirt,
            BlockName.stone,
            BlockName.diorite,
            BlockName.andesite,
            BlockName.grass_block,
            BlockName.dirt_path,
            BlockName.gravel,
            BlockName.sand,
            BlockName.sandstone,
            BlockName.bedrock,
            BlockName.granite,
            BlockName.mossy_cobblestone,
            BlockName.spawner,
            BlockName.obsidian,
            BlockName.clay,
            BlockName.chest,
            BlockName.end_portal_frame,
            BlockName.end_portal
        };

            // Add groups, eg all ores
            string[] keyWords = new string[]
            {
            "ore"
            };

            foreach (BlockName blockName in AllBlockNames)
            {
                for (int i = 0; i < keyWords.Length; i++)
                {
                    if (blockName.ToString().Contains(keyWords[i]))
                    {
                        groundBlocks.Add(blockName);
                        break;
                    }
                }

            }

            return groundBlocks;
        }

        /// <summary>
        /// Is the block a block that would only be seen in the ground
        /// </summary>
        /// <param name="block">Block to test</param>
        /// <param name="includeUndergroundAir">Include cave air and void air as underground blocks</param>
        /// <param name="includeWater">Include water as underground block</param>
        /// <returns>True if the block is a block found in the ground</returns>
        public static bool IsGroundBlock(BlockName block, bool includeUndergroundAir, bool includeWater, bool includePlants)
        {
            bool isGround = GroundBlocks.Contains(block);
            bool isAir = false;
            bool isWater = false;
            bool isPlant = false;

            if (includeUndergroundAir)
            {
                isAir = (block == BlockName.cave_air) || (block == BlockName.void_air);
            }

            if (includeWater)
            {
                isWater = (block == BlockName.water);
            }

            if (includePlants)
            {
                isPlant = PlantBlocks.Contains(block);
            }

            return isGround || isAir || isWater || isPlant;
        }
    }
}