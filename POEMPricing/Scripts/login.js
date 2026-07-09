    document.getElementById('btnLogin').addEventListener('click', async function () {
        const email = document.getElementById('txtEmail').value.trim();
    const password = document.getElementById('txtPassword').value;

    const errorDiv = document.getElementById('loginError');
    errorDiv.style.display = 'none';
    errorDiv.textContent = '';

    if (!email || !password) {
        errorDiv.textContent = 'Email and password are required.';
    errorDiv.style.display = 'block';
    return;
        }

    try {
            const response = await fetch(webRoot + '/api/login/login', {
        method: 'POST',
    headers: {
        'Content-Type': 'application/json'
                },
    body: JSON.stringify({email, password})
            });

    if (!response.ok) {
                const errorData = await response.json().catch(() => null);
    errorDiv.textContent = errorData?.message || 'Invalid email or password.';
    errorDiv.style.display = 'block';
    return;
            }

    const data = await response.json();

    // Example: you might store token or redirect
    // localStorage.setItem('token', data.token);
        window.location.href = '/SKU/Index'; 

        } catch (e) {
        errorDiv.textContent = 'An error occurred while logging in.';
    errorDiv.style.display = 'block';
        }
    });

    //document.getElementById('lnkForgotPassword').addEventListener('click', function (e) {
    //    e.preventDefault();
    //const email = document.getElementById('txtEmail').value.trim();
    //if (!email) {
    //    alert('Please enter your email before using forgot password.');
    //return;
    //    }

    //// Call API for forgot password
    //fetch(webRoot + '/api/login/forgot-password', {
    //    method: 'POST',
    //headers: {'Content-Type': 'application/json' },
    //body: JSON.stringify({email})
    //    }).then(r => {
    //        if (r.ok) {
    //    alert('If an account exists with this email, a reset link has been sent.');
    //        } else {
    //    alert('Error processing forgot password request.');
    //        }
    //    }).catch(() => {
    //    alert('Error processing forgot password request.');
    //    });
    //});

//new
$("#lnkForgotPassword").on("click", function (e) {

    e.preventDefault();

    clearForgotPasswordModal();

    $("#forgotPasswordModal").modal("show");

});

$("#btnSendCode").on("click", function () {

    const email = $("#txtResetEmail").val().trim();

    if (email === "") {

        showMessage("Error", "Please enter your registered email.");

        return;
    }

    $("#btnSendCode")
        .prop("disabled", true)
        .text("Sending...");

    $("#txtResetEmail").prop("readonly", true);

    $.ajax({

        url: webRoot + "api/login/send-reset-code",

        type: "POST",

        contentType: "application/json",

        data: JSON.stringify(email),

        success: function (response) {

            showMessage(
                "Success",
                "Verification code has been sent to your email."
            );

            $("#resetSection").slideDown();

            $("#txtVerificationCode").focus();

            $("#btnSendCode")
                .hide()
                .prop("disabled", false)
                .text("Send Code");

            $("#btnResetPassword").show();

        },

        error: function () {

            showMessage(
                "Error",
                "Unable to send verification code. Please try again."
            );

            $("#txtResetEmail").prop("readonly", false);

            $("#btnSendCode")
                .prop("disabled", false)
                .text("Send Code");

        }

    });

});



$("#btnResetPassword").on("click", function () {

    const email = $("#txtResetEmail").val().trim();

    const code = $("#txtVerificationCode").val().trim();

    const password = $("#txtNewPassword").val();

    const confirmPassword = $("#txtConfirmPassword").val();

    if (!email ||
        !code ||
        !password ||
        !confirmPassword) {

        showMessage("Error", "Please fill all fields.");
        return;
    }

    $.ajax({

        url: webRoot + "api/login/reset-password",

        type: "POST",

        contentType: "application/json",

        data: JSON.stringify({
            email: email,
            code: code,
            password: password,
            confirmPassword: confirmPassword
        }),

        success: function (response) {

            $("#forgotPasswordModal").modal("hide");

            clearForgotPasswordModal();

            $("#txtEmail").val(email);

            showMessage("Success", response.Message);

        }
        ,
        error: function (xhr) {

            let message = "Unable to send verification code. ";

            if (xhr.responseText) {
                message = xhr.responseText;
            }

            showMessage("Error", message);

            $("#txtResetEmail").prop("readonly", false);

            $("#btnSendCode")
                .prop("disabled", false)
                .text("Send Code");
        }

    });

});
function clearForgotPasswordModal() {

    $("#txtResetEmail").val("");
    $("#txtResetEmail").prop("readonly", false);

    $("#txtVerificationCode").val("");

    $("#txtNewPassword").val("");

    $("#txtConfirmPassword").val("");

    $("#resetSection").hide();

    $("#btnResetPassword").hide();

    $("#btnSendCode").show();

}

function showMessage(title, message) {

    $("#messageTitle").text(title);

    $("#messageBody").html(message);

    const modal =
        new bootstrap.Modal(
            document.getElementById("messageModal"));

    modal.show();
}