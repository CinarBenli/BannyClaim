using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BannyClaim
{
    public class Helpers
    {
        public static void DoForeachBarricade(Action<BarricadeData, BarricadeDrop, ItemAsset> action)
        {
            var bRegions = BarricadeManager.regions.Clone() as BarricadeRegion[,];
            foreach (var region in bRegions)
            {
                var barricades = region.barricades.ToList();
                var drops = region.drops.ToList();

                for (int i = 0; i < barricades.Count; i++)
                {
                    action.Invoke(barricades[i], drops[i], drops[i].asset as ItemAsset);
                }
            }
        }

        public static void DoForeachStructure(Action<StructureData, StructureDrop> action)
        {
            var sRegions = StructureManager.regions.Clone() as StructureRegion[,];

            foreach (var region in sRegions)
            {
                var structures = region.structures.ToList();
                var drops = region.drops.ToList();

                for (int i = 0; i < structures.Count; i++)
                {
                    action.Invoke(structures[i], drops[i]);
                }
            }
        }

        public static bool CheckIsGenerated(Vector3 pos, bool requireFuel)
        {
            List<RegionCoordinate> regions = new List<RegionCoordinate>();
            Regions.getRegionsInRadius(pos, PowerTool.MAX_POWER_RANGE, regions);

            float sqrRadius = PowerTool.MAX_POWER_RANGE * PowerTool.MAX_POWER_RANGE;

            List<Transform> barricades = new List<Transform>();
            BarricadeManager.getBarricadesInRadius(pos, sqrRadius, barricades);
            BarricadeManager.getBarricadesInRadius(pos, sqrRadius, regions, barricades);

            foreach (var transform in barricades)
            {
                var generator = transform.GetComponent<InteractableGenerator>();
                if (generator == null)
                    continue;

                if ((generator.transform.position - pos).sqrMagnitude < generator.sqrWirerange)
                {
                    if (requireFuel && !generator.isPowered || generator.fuel <= 0)
                    {
                        continue;
                    }

                    return true;
                }
            }

            return false;
        }
    }
}

