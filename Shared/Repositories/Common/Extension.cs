using WebAppAssembly.Shared.Entities.OnlineStore.Orders;
using OrderItemRequest = WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders.OrderItem;
using PersonalItemRequest = WebAppAssembly.Shared.Entities.Api.Common.PersonalData.PersonalItem;
using OrderItem = WebAppAssembly.Shared.Entities.OnlineStore.Orders.OrderItem;
using PersonalItem = WebAppAssembly.Shared.Entities.OnlineStore.Personals.PersonalItem;
using WebAppAssembly.Shared.Entities.Api.Common.OfTelegram;
using System.Text;
using WebAppAssembly.Shared.Entities.Api.Common.OfTelegram.NativePopupParams;

namespace WebAppAssembly.Shared.Repositories.Common
{
    public static class Extension
    {
        public static ICollection<IOrderItemProcessing> ToOrderItems(this IEnumerable<OrderItemRequest> request)
        {
            var items = new List<IOrderItemProcessing>();
            
            foreach (var item in request)
            {
                items.Add(new OrderItem(item.ProductId, item.ProductName, item.Modifiers, item.Price,
                    item.TotalPrice, item.TotalPriceOfModifiers, item.PositionId, item.Type,
                    item.Amount, item.ProductSizeId, item.ComboInformation, item.Comment,
                    item.SimpleGroupModifiers, item.SimpleModifiers));
            }

            return items;
        }

        public static PersonalItem ToPersonalItem(this PersonalItemRequest request)
            => new (request.DeliveryServiceType, request.TerminalId, request.DeliveryTerminal,
                request.Address);

        public static string EnumToString(this HapticFeedbackImpactOccurredType type)
            => type.ToString().ToLower();

        public static string EnumToString(this PopupButtonType type)
            => type.ToString().ToLower();

        public static string EnumToString(this HapticFeedBackNotificationType type)
            => type.ToString().ToLower();

        public static bool TryToConvertToInvoiceClosedStatus(this string? arg, out InvoiceClosedStatus? status)
        {
            status = null;

            if (!string.IsNullOrWhiteSpace(arg))
            {
                var sb = new StringBuilder(arg);
                sb[0] = arg.ToUpper().First();
                arg = sb.ToString();

                if (!Enum.TryParse(arg, out InvoiceClosedStatus status_))
                {
                    return false;
                }

                status = status_;
                return true;
            }

            return false;
        }

        public static NativePopupParams ToNativePopupParams(this PopupParams popupParams)
        {
            var popupButtons = new List<NativePopupButton>();

            foreach (var popupButton in popupParams.Buttons)
            {
                popupButtons.Add(new NativePopupButton(popupButton.Id, popupButton.Text,
                    popupButton.Type.EnumToString()));
            }

            return new NativePopupParams(popupParams.Title, popupParams.Message, popupButtons);
        }

        public static string ToNutrientFormatForCustomer(this float number)
            => (int)(number * 100) % 100 != 0 ? string.Format("{0:F2}", number) : ((int)number).ToString();
    }
}
