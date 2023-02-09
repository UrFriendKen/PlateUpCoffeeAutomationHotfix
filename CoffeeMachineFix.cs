using Kitchen;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

namespace KitchenCoffeeAutomationHotfix
{
    public class CoffeeMachineFix : StartOfDaySystem
    {
        private const bool DEBUG_OVERRIDE_ENABLED = false;
        private const int COFFEE_MACHINE_APPLIANCE_ID = -1609758240;

        private readonly string[] AFFECTED_VERSIONS = { "1.1.2" };
        private static bool First = true;

        protected override void Initialise()
        {
            base.Initialise();
        }

        protected override void OnUpdate()
        {
            if (DEBUG_OVERRIDE_ENABLED)
                Main.LogInfo("CoffeeMachineFix.OnUpdate()");

            if (First)
            {
                string plateUpVersion = Main.PlateUpVersion;
                Main.LogInfo($"PlateUp v{plateUpVersion}");
                bool isVersionAffected = AFFECTED_VERSIONS.Contains(plateUpVersion);
                string notText = isVersionAffected ? " " : " not ";
                Main.LogInfo($"Version {plateUpVersion} is{notText}affected by Coffee Machine automation bug.");
                if (!isVersionAffected)
                {
                    if (DEBUG_OVERRIDE_ENABLED)
                    {
                        Main.LogInfo("Debug override enabled. Skipping version disable.");
                    }
                    else
                    {
                        Main.LogInfo("Disabling CoffeeAutomationHotfix.");
                        this.Enabled = false;
                        return;
                    }
                }
                First = false;
            }


            EntityQuery ProvidersAppliances = GetEntityQuery(new QueryHelper()
                                              .All(typeof(CAppliance),
                                                   typeof(CItemProvider))
                                              );
            NativeArray<Entity> providersAppliances = ProvidersAppliances.ToEntityArray(Allocator.TempJob);
            foreach (Entity appliance in providersAppliances)
            {
                int applianceId = GetComponent<CAppliance>(appliance).ID;
                CItemProvider itemProvider = GetComponent<CItemProvider>(appliance);
                if (applianceId == COFFEE_MACHINE_APPLIANCE_ID && !itemProvider.PreventReturns)
                {
                    Main.LogInfo($"Coffee Machine found with ID: {appliance.Index}");

                    LogCItemProviderProperties(itemProvider);

                    EntityManager.SetComponentData(appliance, GetUpdatedCItemProvider(itemProvider, preventReturns: true));

                    Main.LogInfo($"Component CItemProvider.PreventReturns for {appliance.Index} set to true.");
                }

            }
            providersAppliances.Dispose();
        }

        private static void LogCItemProviderProperties(CItemProvider p)
        {
            Main.LogInfo("Initial CItemProvider Properties:");
            Main.LogInfo($"Available = {p.Available}");
            Main.LogInfo($"Maximum = {p.Maximum}");
            Main.LogInfo($"DirectInsertionOnly = {p.DirectInsertionOnly}");
            Main.LogInfo($"EmptyAtNight = {p.EmptyAtNight}");
            Main.LogInfo($"PreventReturns = {p.PreventReturns}");
            Main.LogInfo($"DestroyOnEmpty = {p.DestroyOnEmpty}");
            Main.LogInfo($"AutoGrabFromHolder = {p.AutoGrabFromHolder}");
            Main.LogInfo($"AutoPlaceOnHolder = {p.AutoPlaceOnHolder}");
            Main.LogInfo($"AllowRefreshes = {p.AllowRefreshes}");
            Main.LogInfo($"ProvidedItem = {p.ProvidedItem}");
            Main.LogInfo($"ProvidedComponents = {p.ProvidedComponents}");
        }


        private static CItemProvider GetUpdatedCItemProvider(CItemProvider baseCItemProvider,
                                                             int? available = null,
                                                             int? maximum = null,
                                                             bool? directInsertionOnly = null,
                                                             bool? emptyAtNight = null,
                                                             bool? preventReturns = null,
                                                             bool? destroyOnEmpty = null,
                                                             bool? autoGrabFromHolder = null,
                                                             bool? autoPlaceOnHolder = null,
                                                             bool? allowRefreshes = null,
                                                             int? providedItem = null)
        {
            return new CItemProvider
            {
                Available = available == null ? baseCItemProvider.Available : (int)available,
                Maximum = maximum == null ? baseCItemProvider.Maximum : (int)maximum,
                DirectInsertionOnly = directInsertionOnly == null ? baseCItemProvider.DirectInsertionOnly : (bool)directInsertionOnly,
                EmptyAtNight = emptyAtNight == null ? baseCItemProvider.EmptyAtNight : (bool)emptyAtNight,
                PreventReturns = preventReturns == null ? baseCItemProvider.PreventReturns : (bool)preventReturns,
                DestroyOnEmpty = destroyOnEmpty == null ? baseCItemProvider.DestroyOnEmpty : (bool)destroyOnEmpty,
                AutoGrabFromHolder = autoGrabFromHolder == null ? baseCItemProvider.AutoGrabFromHolder : (bool)autoGrabFromHolder,
                AutoPlaceOnHolder = autoPlaceOnHolder == null ? baseCItemProvider.AutoPlaceOnHolder : (bool)autoPlaceOnHolder,
                AllowRefreshes = allowRefreshes == null ? baseCItemProvider.AllowRefreshes : (bool)allowRefreshes,
                ProvidedItem = providedItem == null ? baseCItemProvider.ProvidedItem : (int)providedItem,
                ProvidedComponents = baseCItemProvider.ProvidedComponents
            };
        }
    }
}
