using System.Threading.Tasks;
using Items;
using Player;
using SaveLoad.Interfaces;
using SaveLoad.Tasks;
using Systems.Inv;
using UI.Menu.InventoryMenu;
using UnityEngine;
using UnityEngine.Localization;

namespace TileBehaviours.Container
{
    /// <summary>
    /// The behaviour of a container tile
    /// </summary>
    public class ContainerBehaviour : CustomTileBehaviour, IChainedWorldData
    {
        [SerializeField] private float distanceBeforeAccessDenied;
        [SerializeField] private LocalizedString inventoryName;
        
        public Inventory Inventory { get; private set; }
        private InventoryController _inventoryController;
        private PlayerInventory _playerInventory;
        
        private void Awake()
        {
            Inventory = new Inventory(inventoryName, 15);
            _inventoryController = InventoryController.Current;
            _playerInventory = FindObjectOfType<PlayerInventory>();
        }
        
        private void Update()
        {
            if (Vector2.Distance(_playerInventory.transform.position, transform.position) > distanceBeforeAccessDenied)
            {
                ExitMenu();
            }
        }

        public override void OnInteract()
        {
            if (Vector2.Distance(_playerInventory.transform.position, transform.position) > distanceBeforeAccessDenied) return;
                _inventoryController.SetOpenedInventory(Inventory);
        }

        public override void OnBroken()
        {
            var pos = transform.position;
            
            foreach (var item in Inventory.ItemStacks)
            {
                ItemSpawner.Current.SpawnApproximatelyAt(pos, item);
            }
            
            ExitMenu();
        }

        private void ExitMenu()
        {
            // if currently displaying this inventory, close it when this is broken
            if (_inventoryController.OpenedInventory == Inventory)
            {
                _inventoryController.SetOpenedInventory(null);
            }
        }
        
        public async Task Save(SaveTask saveTask)
        {
            await saveTask.Serialize(Inventory.ItemStacks);
        }

        public async Task Load(LoadTask loadTask)
        {
            Inventory.Load(await loadTask.Deserialize<ItemStack[]>());
        }
    }
}