using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenCoffeeAutomationHotfix
{
    public class CoffeeMachineFix : NightSystem, IModSystem
    {
        private const int COFFEE_MACHINE_APPLIANCE_ID = -1609758240;

        protected override void Initialise()
        {
            base.Initialise();
        }

        protected override void OnUpdate()
        {
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
                    EntityManager.SetComponentData(appliance, GetUpdatedCItemProvider(itemProvider, preventReturns: true));
                    Main.LogInfo($"Component CItemProvider.PreventReturns for {appliance.Index} set to true.");
                }
            }
            providersAppliances.Dispose();
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
