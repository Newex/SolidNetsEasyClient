document.addEventListener('DOMContentLoaded', function () {
  const urlParams = new URLSearchParams(window.location.search);
  const paymentId = urlParams.get('paymentId');
    const checkoutOptions = {
        checkoutKey: document.getElementById('checkout').value,
        paymentId: paymentId,
        containerId: "checkout-container-div",
        language: "da-DK"
    };
    const checkout = new Dibs.Checkout(checkoutOptions);
    checkout.on('payment-completed', function (response) {
        window.location = 'completed.html';
    });
});