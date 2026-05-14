// ======================================================
// validate-sku.js
// FIELD-LEVEL VALIDATION MODULE
// ======================================================


// ======================================================
// 1. FIELD‑LEVEL ERROR HIGHLIGHTING HELPERS
// ======================================================
//function setFieldError($el, message) {
//    $el.addClass('input-error');
//    $el.attr('data-error', message);
//}

//function clearFieldError($el) {
//    $el.removeClass('input-error');
//    $el.removeAttr('data-error');
//}

$(document).on('keypress', '.text-end', function (e) {
    let char = String.fromCharCode(e.which);

    if (!/[0-9.]/.test(char)) {
        e.preventDefault();
    }

    // prevent multiple dots
    if (char === '.' && $(this).val().includes('.')) {
        e.preventDefault();
    }
});

function setFieldError($el, message) {
    // Add Bootstrap invalid class
    $el.addClass('is-invalid');

    // If feedback element doesn't exist, create it
    let $feedback = $el.next('.invalid-feedback');
    if ($feedback.length === 0) {
        $feedback = $('<div class="invalid-feedback"></div>');
        $el.after($feedback);
    }

    // Set message and show
    $feedback.text(message).show();
}

function clearFieldError($el) {
    $el.removeClass('is-invalid');

    // Hide feedback if exists
    let $feedback = $el.next('.invalid-feedback');
    if ($feedback.length > 0) {
        $feedback.hide();
    }
}


// ======================================================
// 2. FIELD‑LEVEL VALIDATION RULES
// ======================================================
const FieldValidators = {

    // ======================================================
    // SKU INFORMATION - Vendor Product
    // ======================================================
    
    '#ddlCompany': $el =>
        !$el.val().trim() ? "Company is required." : null,

    '#ddlVendor': $el =>
        !$el.val().trim() ? "Vendor is required." : null,

    '#ddlOrderType': $el =>
        !$el.val().trim() ? "Order Type is required." : null,

    //'#txtVendorNumber': $el =>
    //    !$el.val().trim() ? "Vendor Number is required." : null,

    '#txtSKUNumber': $el =>
        !$el.val().trim() ? "SKU Number is required." : null,


    '#ddlCategory': $el =>
        !$el.val().trim() ? "Category is required." : null,

    '#ddlSubCategory': $el =>
        !$el.val().trim() ? "Sub Category is required." : null,

    //'#txtSizeLength': $el =>
    //    !$el.val().trim() ? "Size/Length is required." : null,


    //'#txtSizeLength': $el => {
    //    if (!$el.val().trim()) return "Size/Length is required.";
    //    if (isNaN($el.val())) return "Size/Length must be numeric.";
    //    return null;
    //},

    // ======================================================
    // SKU INFORMATION - METAL 
    // ======================================================
    '#ddlMetal': $el =>
        !$el.val().trim() ? "Metal is required." : null,

    '#ddlKarat': $el => {
        const metal = $('#ddlMetal').val();
        if (metal === 'Gold') {
            !$el.val().trim() ? "Karat is required." : null
        } else {null }
            },

    '#ddlMetalColor': $el =>
        !$el.val().trim() ? "Metal Color is required." : null,

    '#txtMetalGmWt': $el =>
        !$el.val().trim() ? "Metal Weight (gmWt) is required." : null,
    '.metal-gmwt': $el => {
        const v = parseFloat($el.val());
        return (isNaN(v) || v <= 0) ? "gmWt must be > 0." : null;
    },

    '.metal-rate': $el => {
        const v = parseFloat($el.val());
        return (isNaN(v) || v <= 0) ? "Rate must be > 0." : null;
    },

    // ======================================================
    // SKU INFORMATION - FINDINGS
    // ======================================================
    '#ddlFindingSupplier': $el =>
        !$el.val().trim() ? "Finding Supplier is required." : null,

    '#txtFindingSku': $el =>
        !$el.val().trim() ? "Finding SKU is required." : null,

    '#ddlFindingAssembly': $el =>
        !$el.val().trim() ? "Finding Assembly is required." : null,

    '#txtFindingQty': $el =>
        !$el.val().trim() ? "Finding Quantity is required." : null,

    // ======================================================
    // STONE INFORMATION
    // ======================================================
    '#ddlStoneVendor': $el =>
        !$el.val().trim() ? "Stone Vendor is required." : null,

    '#ddlStoneType': $el =>
        !$el.val().trim() ? "Stone Type is required." : null,

    '#ddlGrowing': $el =>
        !$el.val().trim() ? "Growing Type is required." : null,

    '#ddlSettingLocation': $el =>
        !$el.val().trim() ? "Setting Location is required." : null,

    '#ddlStoneShape': $el =>
        !$el.val().trim() ? "Stone Shape is required." : null,

    '#txtStoneMMSize': $el =>
        !$el.val().trim() ? "Stone MM Size is required." : null,

    '#txtStoneQty': $el =>
        !$el.val().trim() ? "Stone Quantity is required." : null,

    '#txtTotalAdjStoneWt': $el =>
        !$el.val().trim() ? "Total Adjusted Stone Weight is required." : null,

    '#ddlStoneQuality': $el =>
        !$el.val().trim() ? "Stone Quality is required." : null,
    '.stone-mm': $el => {
        const v = parseFloat($el.val());
        return (isNaN(v) || v <= 0) ? "MM Size must be > 0." : null;
    },

    '.stone-qty': $el => {
        const v = parseInt($el.val());
        return (isNaN(v) || v <= 0) ? "Qty must be > 0." : null;
    },

    // ======================================================
    // STONE INFORMATION - SETTING
    // ======================================================
    '#ddlSettingVendor': $el =>
        !$el.val().trim() ? "Setting Vendor is required." : null,

    '#ddlSettingType': $el =>
        !$el.val().trim() ? "Setting Type is required." : null,

    '#txtSemiMinWt': $el =>
        !$el.val().trim() ? "Semi Minimum Weight is required." : null,

    '#txtCenterMinWt': $el =>
        !$el.val().trim() ? "Center Minimum Weight is required." : null,

    // ======================================================
    // Labor
    // ======================================================
    '#txtCastPcs': $el => {
        const v = parseInt($el.val());
        return (isNaN(v) || v <= 0) ? "Cast pieces must be > 0." : null;
    }
};


// ======================================================
// 3. ATTACH BLUR VALIDATION TO FIELDS
// ======================================================
Object.keys(FieldValidators).forEach(selector => {
    $(document).on('blur', selector, function () {
        const $el = $(this);
        //const validator = FieldValidators[selector];
        //const error = validator($el);

        //if (error) {
        //    setFieldError($el, error);
        //   $el.focus();
        //} else {
        //    clearFieldError($el);
        //}
        clearFieldError($el);
    });
});


// ======================================================
// 4. OPTIONAL: SHOW ERROR MESSAGE ON HOVER
// ======================================================
$(document).on('mouseenter', '.input-error', function () {
    const msg = $(this).attr('data-error');
    if (msg) $(this).attr('title', msg);
});

// Debounce utility to avoid spamming API
function debounce(fn, ms) {
    let t;
    return (...args) => {
        clearTimeout(t);
        t = setTimeout(() => fn.apply(this, args), ms);
    };
}

// Async API check (returns true if exists)
async function skuExistsAsync(skuNumber) {
    const url = webRoot + '/api/sku/exists/' + encodeURIComponent(skuNumber);
    const res = await fetch(url, { method: 'GET' });
    const data = await res.json();
    return !!data.Exists;
}

// Validate SKU number with server-side uniqueness check
async function validateSkuNumber() {
    const id = 'txtSKUNumber';
    const val = (document.getElementById(id)?.value || '').trim();

    // Required check first
    if (!val) {
        setFieldError($('#txtSKUNumber'), 'SKU Number is required.');
        return false;
    }

    // Server uniqueness check
    try {
        const exists = await skuExistsAsync(val);
        if (exists) {
            setFieldError($('#txtSKUNumber'), 'SKU Number already exists.');
            return false;
        }        
        return true;
    } catch (e) {
        //setFieldError($('#txtSKUNumber'), 'Unable to validate SKU.');
        console.error('SKU validation error:', e);
        return false;
    }
}

// Wire up events (debounced on input, definitive on blur)
const debouncedSkuCheck = debounce(validateSkuNumber, 300);

// Only attach SKU validation if not on /SKU/Edit or /SKU/Info
const path = window.location.pathname.toLowerCase();

if (!(path.includes("/sku/edit") || path.includes("/sku/info"))) {
    document.getElementById('txtSKUNumber')?.addEventListener('input', () => {
        // Clear error while typing; re-check after debounce
        clearFieldError($('#txtSKUNumber'));
        debouncedSkuCheck();
    });
}



document.getElementById('txtSKUNumber')?.addEventListener('blur', () => {
    validateSkuNumber(); // final check when user leaves field
});
    
// Validate Adjusted Total Stone Weight based on Per Stone Weight and Quantity Line Items


const txtPerStoneWt = document.getElementById("txtPerStoneWt");
const txtQty = document.getElementById("txtStoneQty");
const txtTotalAdjStoneWt = document.getElementById("txtTotalAdjStoneWt");

const txtSemiMinWtVal = document.getElementById("txtSemiMinWt");
const txtSemiAdjWtVal = document.getElementById("txtSemiAdjWt");

const txtCenterMinWtVal = document.getElementById("txtCenterMinWt");
const txtCenterAdjWtVal = document.getElementById("txtCenterAdjWt");

const stoneRules = [
    { min: 0.0001, max: 0.57, per: 0.10 },
    { min: 0.58, max: 1.49, per: 0.07 },
    { min: 1.5, max: 2.99, per: 0.05 },
    { min: 3, max: 4.99, per: 0.04 },
    { min: 5, max: 9.99, per: 0.03 },
    { min: 10, max: 20, per: 0.02 }
];

/**
 * Generic validation function
 * adjInput - the adjusted weight textbox being validated
 * baseWt - the base weight (perStoneWt*qty OR semiMinWt OR centerMinWt)
 */
function validateAdjStoneGeneric(adjInput, baseWt) {
    const adjValue = parseFloat(adjInput.value) || 0;

    // Find matching rule based on base weight
    const rule = stoneRules.find(r => baseWt >= r.min && baseWt <= r.max);

    if (!rule) {
        setFieldError($(adjInput), "Base weight not in any defined range.");
        return;
    }

    // Allowed range with 4 decimal precision
    const minAllowed = parseFloat((baseWt - (baseWt * rule.per)).toFixed(4));
    const maxAllowed = parseFloat((baseWt + (baseWt * rule.per)).toFixed(4));

    console.log("Range:", minAllowed, "-", maxAllowed, "Adj:", adjValue);

    if (adjValue < minAllowed || adjValue > maxAllowed) {
        setFieldError($(adjInput),
            `Value must be between ${minAllowed} and ${maxAllowed}`);
    } else {
        clearFieldError($(adjInput));
    }
}

// --- Event bindings ---

// Total Adj Stone Wt → base = perStoneWt * qty
function validateTotalAdj() {
    const perStoneWt = parseFloat(txtPerStoneWt.value) || 0;
    const qty = parseInt(txtQty.value, 10) || 0;
    const baseWt = perStoneWt * qty;
    validateAdjStoneGeneric(txtTotalAdjStoneWt, baseWt);
}
txtTotalAdjStoneWt.addEventListener("input", validateTotalAdj);
txtTotalAdjStoneWt.addEventListener("blur", validateTotalAdj);

// Semi Adj Wt → base = SemiMinWt
function validateSemiAdj() {
    const baseWt = parseFloat(txtSemiMinWtVal.value) || 0;
    validateAdjStoneGeneric(txtSemiAdjWtVal, baseWt);
}
txtSemiAdjWtVal.addEventListener("input", validateSemiAdj);
txtSemiAdjWtVal.addEventListener("blur", validateSemiAdj);

// Center Adj Wt → base = CenterMinWt
function validateCenterAdj() {
    const baseWt = parseFloat(txtCenterMinWtVal.value) || 0;
    validateAdjStoneGeneric(txtCenterAdjWtVal, baseWt);
}
txtCenterAdjWtVal.addEventListener("input", validateCenterAdj);
txtCenterAdjWtVal.addEventListener("blur", validateCenterAdj);