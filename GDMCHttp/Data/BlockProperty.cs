using System;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp.Data
{
    /// <summary>
    /// All block property names
    /// </summary>
    public enum BlockProperty
    {
        type,
        waterlogged,
        facing,
        half,
        shape,
        rotation,
        west,
        east,
        up,
        south,
        north,
        in_wall,
        powered,
        open,
        distance,
        bottom,
        persistent,
        age,
        down,
        occupied,
        part,
        face,
        axis,
        disarmed,
        attached,
        lit,
        inverted,
        power,
        hinge,
        extended,
        hanging,
        moisture,
        mode,
        honey_level,
        triggered,
        conditional,
        drag,
        stage,
        attachment,
        orientation,
        unstable,
        pickles,
        layers,
        has_bottle_0,
        has_bottle_1,
        has_bottle_2,
        bites,
        snowy,
        leaves,
        has_book,
        signal_fire,
        delay,
        locked,
        level,
        eye,
        has_record,
        // @ allows for keyword - https://stackoverflow.com/questions/10688863/c-sharp-enums-with-reserved-keywords
        @short,
        charges,
        eggs,
        hatch,
        note,
        instrument,
        enabled
    }
}
