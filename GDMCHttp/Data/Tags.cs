using System;

namespace GDMCHttp.Data
{
    /// <summary>
    /// Enum for all minecraft tags. The names are identical to what is found in minecraft except that '/'s are now '_'
    /// </summary>
    [Flags]
    public enum Tags
    {
        fire,
        piglin_repellents,
        dragon_transparent,
        nether_carver_replaceables,
        goats_spawnable_on,
        mineable_pickaxe,
        dripstone_replaceable_blocks,
        lush_ground_replaceable,
        moss_replaceable,
        overworld_carver_replaceables,
        azalea_root_replaceable,
        snaps_goat_horn,
        sculk_replaceable_world_gen,
        base_stone_overworld,
        stone_ore_replaceables,
        sculk_replaceable,
        ancient_city_replaceable,
        deepslate_ore_replaceables,
        valid_spawn,
        azalea_grows_on,
        dead_bush_may_place_on,
        wolves_spawnable_on,
        big_dripleaf_placeable,
        bamboo_plantable_on,
        mineable_shovel,
        foxes_spawnable_on,
        frogs_spawnable_on,
        parrots_spawnable_on,
        enderman_holdable,
        animals_spawnable_on,
        dirt,
        rabbits_spawnable_on,
        convertable_to_mud,
        mushroom_grow_block,
        snow_layer_can_survive_on,
        mangrove_roots_can_grow_through,
        mangrove_logs_can_grow_through,
        nylium,
        mineable_axe,
        planks,
        non_flammable_wood,
        saplings,
        flowers,
        wither_immune,
        dragon_immune,
        lava_pool_stone_cannot_replace,
        features_cannot_replace,
        geode_invalid_blocks,
        infiniburn_end,
        sand,
        coal_ores,
        iron_ores,
        needs_stone_tool,
        copper_ores,
        gold_ores,
        guarded_by_piglins,
        needs_iron_tool,
        redstone_ores,
        emerald_ores,
        lapis_ores,
        diamond_ores,
        needs_diamond_tool,
        crystal_sound_blocks,
        beacon_base_blocks,
        stairs,
        slabs,
        logs_that_burn,
        logs,
        oak_logs,
        completes_find_tree_tutorial,
        overworld_natural_logs,
        spruce_logs,
        birch_logs,
        jungle_logs,
        acacia_logs,
        dark_oak_logs,
        mangrove_logs,
        crimson_stems,
        warped_stems,
        leaves,
        mineable_hoe,
        impermeable,
        fall_damage_resetting,
        replaceable_plants,
        underwater_bonemeals,
        occludes_vibration_signals,
        wool,
        dampens_vibrations,
        small_flowers,
        hoglin_repellents,
        climbable,
        small_dripleaf_placeable,
        frog_prefer_jump_to,
        wooden_slabs,
        wall_post_override,
        snow,
        inside_step_sound_blocks,
        ice,
        snow_layer_cannot_survive_on,
        polar_bears_spawnable_on_alternate,
        axolotls_spawnable_on,
        wooden_fences,
        fences,
        infiniburn_nether,
        infiniburn_overworld,
        base_stone_nether,
        soul_speed_blocks,
        wither_summon_base_blocks,
        soul_fire_base_blocks,
        stone_bricks,
        mooshrooms_spawnable_on,
        wooden_stairs,
        walls,
        anvil,
        terracotta,
        wool_carpets,
        tall_flowers,
        wart_blocks,
        shulker_boxes,
        coral_blocks,
        coral_plants,
        corals,
        buttons,
        wooden_buttons,
        pressure_plates,
        stone_pressure_plates,
        wooden_pressure_plates,
        doors,
        wooden_doors,
        trapdoors,
        wooden_trapdoors,
        unstable_bottom_center,
        fence_gates,
        prevent_mob_spawning_inside,
        rails,
        crops,
        bee_growables,
        signs,
        standing_signs,
        beds,
        cauldrons,
        flower_pots,
        banners,
        campfires,
        beehives,
        candles,
        strider_warm_blocks,
        wall_signs,
        portals,
        wall_corals,
        candle_cakes,
        cave_vines
    }

    public static class FlagUtils
    {
        public static bool TryConvertToTag(string value, out Tags tag)
        {
            string formatted = value.Replace("minecraft:", "").Replace('/', '_');
            return Enum.TryParse(formatted, out tag);
        }

        public static bool TryConvertToCompoundTag(string[] values, out Tags tag)
        {
            tag = new Tags();
            for (int i = 0; i < values.Length; i++)
            {
                Tags current;
                if (!TryConvertToTag(values[i], out current))
                {
                    return false;
                } else
                {
                    tag |= current;
                }
            }
            return true;
        }
    }
}