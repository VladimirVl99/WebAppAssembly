// object of Telegram app
const tlg = window.Telegram.WebApp;
const allowColor = "#55be61"; // #00A86B

// Informs the Telegram app that the Web App is ready
function TelegramWebAppInit() {
    console.log("Initialization of the Telegram Bot has started");
    tlg.ready();
    tlg.enableClosingConfirmation();
    return tlg.initDataUnsafe.user.id;
}

function SetMainButtonColor(color) {
    allowColor = color;
}

// Close Web App
function CloseWebApp() {
    console.log("Web App has been closed");
    tlg.close();
}

// Set 'View' main button
function SetMainButtonText(txt, isExpand) {
    if (isExpand) {
        tlg.expand();
    }
    tlg.MainButton.setParams({ "text": txt, "color": allowColor, "is_active": true, "is_visible": true });
}

// Hide the main button
function HideMainButton() {
    tlg.MainButton.hide();
}

// Gets the main button event
async function MainButtonHandler() {
    await MainButtonHandlerPromise();
    tlg.HapticFeedback.impactOccurred("soft");
}

// Gets the main button event
function MainButtonHandlerPromise() {
    return new Promise((resolve, reject) => {
        tlg.onEvent('mainButtonClicked', callback => {
            resolve(true);
        });
    });
}

// Show the back button
function ShowBackButton() {
    tlg.BackButton.show();
}

// Hide the back button
function HideBackButton() {
    tlg.BackButton.hide();
}

// Gets the back button event
async function BackButtonHandler() {
    await BackButtonHandlerPromise();
    tlg.HapticFeedback.impactOccurred("soft");
}

// Gets the back button event
function BackButtonHandlerPromise() {
    return new Promise((resolve, reject) => {
        tlg.onEvent('backButtonClicked', callback => {
            resolve(true);
        });
    });
}

// Gets the invoice events
async function InvoiceClosedHandler(invoiceLink) {
    return await InvoiceClosedHandlerPromise(invoiceLink);
}

// Gets the invoice events
function InvoiceClosedHandlerPromise(invoiceLink) {
    return new Promise((resolve, reject) => {
        tlg.openInvoice(invoiceLink, callback => {
            resolve(callback);
        });
    });
}

// Set haptic feedback notification
function SetHapticFeedbackNotification(type) {
    tlg.HapticFeedback.notificationOccurred(type);
}

function SetHapticFeedbackImpactOccurred(style) {
    tlg.HapticFeedback.impactOccurred(style);
}

// Set haptic feedback of the 'selection changed'
function SetHapticFeedbackSelectionChanged() {
    tlg.HapticFeedback.selectionChanged();
}

// Set 'Ok' message
function SetOkPopupMessage(title, message) {
    tlg.showPopup({ "title": title, "message": message, "buttons": [{ "type": "ok" }] });
}

function ShowProgress() {
    tlg.MainButton.showProgress(false);
}

function HideProgress() {
    tlg.MainButton.hideProgress();
}

function ScrollToTop() {
    //window.scrollBy(0, -window.innerHeight);
    //window.scrollBy({top:0});
    window.scrollTo(0, 0);
}

//
async function RemoveSelectedProductsWithMofifiers(txt) {
    var promise = new Promise((resolve, reject) => {
        tlg.showPopup({
            "message": txt, "buttons": [{ "id": "yes", "type": "destructive", "text": "Да" },
            { "id": "no", "type": "destructive", "text": "Нет" }]
        }, callback => {
            resolve(callback);
        });
    });
    var result = await promise;
    tlg.HapticFeedback.impactOccurred("soft");
    if (result == 'yes') return true;
    else return false;
}

async function ShowPopupParamsAsync(popup) {
    var promise = new Promise((resolve, reject) => {
        tlg.showPopup(popup, callback => {
            resolve(callback);
        });
    });
    return await promise;
}

// Just test
function TestFunction() {
    tlg.showPopup({ "title": "Предупреждение", "message": "Сумма заказа слишком мала", "buttons": [{ "id": "1", "type": "ok" }] });
}