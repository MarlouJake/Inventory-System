using InventorySystem.Utilities;
using System.ComponentModel;

namespace InventorySystem.Models.Page
{
    public class ErrorViewModel
    {
        [DisplayName(DisplayNames.RequestId)]
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
