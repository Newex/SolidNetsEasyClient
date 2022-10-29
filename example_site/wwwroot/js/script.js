document.addEventListener('DOMContentLoaded', function () {
    const checkoutOptions = {
        checkoutKey: document.getElementById('checkout').value,
        paymentId: document.getElementById('payment').value,
        containerId: "checkout-container-div",
    };
    const checkout = new Dibs.Checkout(checkoutOptions);
    checkout.on('payment-completed', function (response) {
        window.location = 'completed.html';
    });
});