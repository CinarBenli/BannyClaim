using Rocket.API;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BannyClaim
{
    public class Class1 : RocketPlugin
    {
        protected override void Load()
        {
            base.Load();

        }



        [RocketCommand("barikat", "test", "test", Rocket.API.AllowedCaller.Both)]
        public void Test(IRocketPlayer caller, string[] command)
        {
            StartCoroutine(Test());
        }
        public IEnumerator<WaitForSeconds> Test()
        {
            while (true)
            {
                Helpers.DoForeachBarricade((data, drop, asset) =>
                {
                    Vector3 position = data.point;
                    bool shouldGeneratorsRequiredFuel = false;

                    if (Helpers.CheckIsGenerated(position, shouldGeneratorsRequiredFuel))
                    {
                        return;
                    }
                    else
                    {
                        BarricadeManager.damage(drop.model.transform, 1, 3, true, CSteamID.Nil, EDamageOrigin.Unknown);
                    }
                });

                Helpers.DoForeachStructure((data, asset) =>
                {
                    Vector3 position = data.point;
                    bool shouldGeneratorsRequiredFuel = false;
                    var structure = StructureManager.FindStructureByRootTransform(asset.model.transform);
                    var datas = structure.GetServersideData();

                    if (Helpers.CheckIsGenerated(position, shouldGeneratorsRequiredFuel))
                    {
                        return;
                    }
                    else
                    {
                        StructureManager.damage(structure.model.transform, structure.model.position, 1, 3, true, CSteamID.Nil, EDamageOrigin.Unknown);
                    }
                });
                yield return new WaitForSeconds(3);
            }
        }






    }
}
