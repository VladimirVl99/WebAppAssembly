﻿// object of Telegram app
const tlg = window.Telegram.WebApp;
const allowColor = "#55be61"; // #00A86B

// Informs the Telegram app that the Web App is ready
function InitTelegram() {
    console.log("Initialization of the Telegram Bot has started");
    tlg.ready();
    tlg.enableClosingConfirmation();
    return tlg.initDataUnsafe.user.id;
}

// Close Web App
function CloseWebApp() {
    console.log("Web App has been closed");
    tlg.close();
}

// Set 'View' main button
function SetViewOrderButton() {
    tlg.MainButton.setParams({ "text": "Корзина", "color": allowColor, "is_active": true, "is_visible": true });
}

// Set 'View' main button
function SetMainButtonText(txt) {
    tlg.MainButton.setParams({ "text": txt, "color": allowColor, "is_active": true, "is_visible": true });
}

// Hide the main button
function HideViewOrderButton() {
    tlg.MainButton.hide();
}

// Show 'Pay' main button
function SetPayOrderButton(sum) {
    tlg.expand();
    tlg.MainButton.setText("Оплатить ₽" + sum);
}

// Show 'Add modifier product' button
function SetSelectProductWithModifiersButton(sum) {
    tlg.expand();
    tlg.MainButton.setParams({ "text": "Добавить ₽" + sum, "color": allowColor, "is_active": true, "is_visible": true });
}

// Gets the main button event
async function HandlerMainButton() {
    var res = await HandlerMainButtonClickedPromise();
    tlg.HapticFeedback.impactOccurred("soft");
}

// Gets the main button event
function HandlerMainButtonClickedPromise() {
    return new Promise((resolve, reject) => {
        tlg.onEvent('mainButtonClicked', callback => {
            resolve(true);
        });
    });
}

// Show the back button
function BackButtonShow() {
    tlg.BackButton.show();
}

// Hide the back button
function BackButtonHide() {
    tlg.BackButton.hide();
}

// Gets the back button event
async function HandlerBackButtonClicked() {
    var res = await HandlerBackButtonClickedPromise();
    tlg.HapticFeedback.impactOccurred("soft");
}

// Gets the back button event
function HandlerBackButtonClickedPromise() {
    return new Promise((resolve, reject) => {
        tlg.onEvent('backButtonClicked', callback => {
            resolve(true);
        });
    });
}

// Gets the invoice events
async function HandlerOpenInvoice(invoiceLink) {
    return await HandlerOpenInvoicePromise(invoiceLink);
}

// Gets the invoice events
function HandlerOpenInvoicePromise(invoiceLink) {
    return new Promise((resolve, reject) => {
        tlg.openInvoice(invoiceLink, callback => {
            resolve(callback);
        });
    });
}

// Set haptic feedback notification
function HapticFeedbackNotificationSet(type) {
    tlg.HapticFeedback.notificationOccurred(type);
}

// Set haptic feedback of the 'selection changed'
function HapticFeedbackSelectionChangedSet() {
    tlg.HapticFeedback.selectionChanged();
}

// Just test
function TestFunction() {
    tlg.showPopup({ "title": "Предупреждение", "message": "Сумма заказа слишком мала", "buttons": [{ "id": "1", "type": "ok" }] });
}

// Set 'Ok' message
function SetOkPopupMessage(title, message, notification_type) {
    tlg.showPopup({ "title": title, "message": message, "buttons": [{ "type": "ok" }] });
    HapticFeedbackNotificationSet(notification_type);
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

async function ShowLog(txt) {
    console.log(txt);
}