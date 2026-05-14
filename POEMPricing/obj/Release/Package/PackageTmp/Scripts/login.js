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

    document.getElementById('lnkForgotPassword').addEventListener('click', function (e) {
        e.preventDefault();
    const email = document.getElementById('txtEmail').value.trim();
    if (!email) {
        alert('Please enter your email before using forgot password.');
    return;
        }

    // Call API for forgot password
    fetch(webRoot + '/api/login/forgot-password', {
        method: 'POST',
    headers: {'Content-Type': 'application/json' },
    body: JSON.stringify({email})
        }).then(r => {
            if (r.ok) {
        alert('If an account exists with this email, a reset link has been sent.');
            } else {
        alert('Error processing forgot password request.');
            }
        }).catch(() => {
        alert('Error processing forgot password request.');
        });
    });

