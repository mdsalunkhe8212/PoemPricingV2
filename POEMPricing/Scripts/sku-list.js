// ---------------------------
// State object to track filters and pagination
// ---------------------------
const state = {
    company: '',       // Selected company filter
    skuPrefix: '',     // SKU number prefix (min length 5 for search)
    category: '',      // Selected category filter
    subcategory: '',   // Selected subcategory filter
    collection: '',    // Selected collection filter
    page: 1,           // Current page number
    pageSize: 10,      // Number of records per page
    totalCount: 0      // Total records count (for pagination)
};

// ---------------------------
// Load SKU list from API with filters and pagination
// ---------------------------
async function loadSkuPage() {
    //if (!state.company) return; // Company is mandatory

    // Build query parameters
    const params = new URLSearchParams({
        company: state.company,
        page: state.page,
        pageSize: state.pageSize
    });
    //params.append('skuPrefix', state.skuPrefix.trim());
    // Apply SKU prefix filter if length >= 5, else use category/subcategory/collection
    if (state.skuPrefix && state.skuPrefix.trim().length >= 5) {
        params.append('skuPrefix', state.skuPrefix.trim());
    } else {
        if (state.category) params.append('category', state.category);
        if (state.subcategory) params.append('subcategory', state.subcategory);
        if (state.collection) params.append('collection', state.collection);
    }

    // Call API
    const res = await fetch(webRoot + '/api/sku/list?' + params.toString());
    const data = await res.json();

    // Update state and render UI
    state.totalCount = data.TotalCount || 0;
    renderTable(data.Items || []);
    renderPager();
}

// ---------------------------
// Render SKU table rows
// ---------------------------

// Helper: Base64 encode
function encryptSku(value) {
    return btoa(value); // encodes string to Base64
}

function renderTable(items) {
    const tbody = document.querySelector('#skuTable tbody');
    tbody.innerHTML = '';

    items.forEach(sku => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${sku.Sku}</td>
            <td>${sku.Top1Metal || ''}</td>
            <td>${(sku.Price1 ?? 0).toFixed(2)}</td>
            <td>${(sku.Price2 ?? 0).toFixed(2)}</td>
            <td>${(sku.Price3 ?? 0).toFixed(2)}</td>
            <td>${sku.ModifiedDate ? new Date(sku.ModifiedDate).toLocaleDateString() : 'Not Modified'}</td>
            <td>${sku.IsActive ? 'Active' : 'Inactive'}</td>
        `;
        // Make entire row clickable
        tr.style.cursor = "pointer"; // show pointer cursor
        tr.addEventListener("click", () => {
            const encryptedSku = encryptSku(sku.Sku.trim());
            window.location.href = "/SKU/Info?skunumber=" + encodeURIComponent(encryptedSku);

        });


        tbody.appendChild(tr);
    });
}

// ---------------------------
// Render pagination controls
// ---------------------------
function renderPager() {
    const pager = document.getElementById('pager');
    pager.innerHTML = '';

    const totalPages = Math.max(1, Math.ceil(state.totalCount / state.pageSize));

    // Helper to create pagination item
    const mkItem = (label, page, disabled = false, active = false) => {
        const li = document.createElement('li');
        li.className = `page-item ${disabled ? 'disabled' : ''} ${active ? 'active' : ''}`;
        const a = document.createElement('a');
        a.className = 'page-link';
        a.href = '#';
        a.textContent = label;
        a.onclick = (e) => {
            e.preventDefault();
            if (!disabled && state.page !== page) {
                state.page = page;
                loadSkuPage();
            }
        };
        li.appendChild(a);
        return li;
    };

    // Previous button
    pager.appendChild(mkItem('«', Math.max(1, state.page - 1), state.page === 1));

    // Page numbers (window size = 5)
    const windowSize = 5;
    const start = Math.max(1, state.page - Math.floor(windowSize / 2));
    const end = Math.min(totalPages, start + windowSize - 1);

    for (let p = start; p <= end; p++) {
        pager.appendChild(mkItem(String(p), p, false, p === state.page));
    }

    // Next button
    pager.appendChild(mkItem('»', Math.min(totalPages, state.page + 1), state.page === totalPages));
}

// ---------------------------
// Event wiring for filters
// ---------------------------

// Company dropdown change → reload list
document.getElementById('ddlCompany').addEventListener('change', e => {
    state.company = e.target.value;
    state.page = 1;    
    loadSkuPage();
});

// SKU number input (min length 5) → disables other dropdowns
document.getElementById('txtSkuNumber').addEventListener('input', e => {
    state.skuPrefix = e.target.value.trim();

    const disableDropdowns = state.skuPrefix.length >= 5;
    document.getElementById('ddlCategory').disabled = disableDropdowns;
    document.getElementById('ddlSubCategory').disabled = disableDropdowns;
    document.getElementById('ddlCollection').disabled = disableDropdowns;

    state.page = 1;
    loadSkuPage();
});

// Category dropdown change → clears/disables SKU input
document.getElementById('ddlCategory').addEventListener('change', e => {
    state.category = e.target.options[e.target.selectedIndex].text || '';
    document.getElementById('txtSkuNumber').value = '';
    document.getElementById('txtSkuNumber').disabled = true;
    state.skuPrefix = '';
    state.page = 1;
});

// Subcategory dropdown change → reload list
document.getElementById('ddlSubCategory').addEventListener('change', e => {
    state.subcategory = e.target.options[e.target.selectedIndex].text || '';
    state.page = 1;
    loadSkuPage();
});

// Collection dropdown change → reload list
document.getElementById('ddlCollection').addEventListener('change', e => {
    state.collection = e.target.options[e.target.selectedIndex].text || '';
    state.page = 1;
    loadSkuPage();
});

// ---------------------------
// Initial load on page ready
// ---------------------------
state.company = document.getElementById('ddlCompany').value || '';
state.page = 1;
//loadSkuPage();

/********************************************************
 * CATEGORY → SUBCATEGORY
 ********************************************************/
$('#ddlCategory').change(function () {
    const parentId = $(this).val();

    $.get(webRoot + '/api/sku/subcategory?parentId=' + parentId, function (data) {
        const subDropdown = $('#ddlSubCategory');
        subDropdown.empty().append('<option value=""></option>');

        $.each(data, function (i, item) {
            subDropdown.append($('<option>').val(item.Key).text(item.Value));
        });
    });
});