/************************************************************
 *  Script Section 1 — GLOBAL VARIABLES & HELPER FUNCTIONS
 *  ---------------------------------------------------------
 *  Contains:
 *   - Global arrays for Findings, Metals, Stones
 *   - Edit index trackers
 *   - Utility helpers (parseNumber, debounce)
 ************************************************************/

// Global arrays for module data
let findingLines = [];
let findings = [];          // alias used in SKU module
let metalLines = [];
let metals = [];            // alias used in SKU module
let stoneList = [];         // populated elsewhere
let totalMetalCost = 0;
let totalFindingCost = 0;
let totalCenterStoneCost = 0;
let totalCenterSettingCost = 0;
let totalSemiStoneCost = 0;
let totalSemiSettingCost = 0;
let totalSemiWt = 0;
let totalCenterWt = 0;
let totalLaborCost = 0;
// Mahesh Start
let vendortotaltax = 0.0;
let vendorDutyper = 0.00;
let vendorDutyVal = 0.00
let vendorTariffPer = 0.00;
let vendorTariffVal = 0.00;
let vendorPenaltyPer = 0.00;
let vendorPenaltyVal = 0.00;

let findingtotaltax = 0.0;
 
let findingDutyper = 0.00
let findingDutyVal = 0.00
let findingTariffPer = 0.00;
let findingTariffVal = 0.00;
let findingPenaltyPer = 0.00;
let findingPenaltyVal = 0.00;

//let laborDutyper = 0.00
//let findingDutyVal = 0.00
//let findingTariffPer = 0.00;
//let findingTariffVal = 0.00;
//let findingPenaltyPer = 0.00;
//let findingPenaltyVal = 0.00;
let labortotaltax = 0.0;
let laborDutyper = 0.00
let LaborDutyVal = 0.00
let laborTariffPer = 0.00;
let LaborTariffVal = 0.00;
let laborPenaltyPer = 0.00;
let LaborPenaltyVal = 0.00;


let settingtotaltax = 0.0;
let settingDutyper = 0.00
let settingDutyVal = 0.00
let settingTariffPer = 0.00;
let settingTariffVal = 0.00;
let settingPenaltyPer = 0.00;
let settingPenaltyVal = 0.00;

let diamondtotalSemitax = 0.0;
let diamondtotalCentertax = 0.0;
let diamondDutyper = 0.00
let diamondDutyVal = 0.00
let diamondTariffPer = 0.00;
let diamondTariffVal = 0.00;
let diamondPenaltyPer = 0.00;
let diamondPenaltyVal = 0.00;

let settingtotalSemitax = 0.0;
let settingtotalCentertax = 0.0;

//let findingTaxper = 0.00;
//let findingTax = 0.00;
//let laborTaxper = 0.00;
//let laborTax = 0.00;
//let diamondTaxper = 0.00;
//let diamondSemiTax = 0.00;
//let diamondCenterTax = 0.00;
//let totalSemiSettingCost = 0.00;
//let totalCenterSettingCost = 0.00;



let totalLaborCostWTDuty = 0;
let totalSemiAdjWt = 0;
let totalCenterAdjWt = 0;
let totalStoneQty = 0;
let totalTotalStoneWt = 0;
let totalTotalAdjStoneWt = 0;
let totalCosttotal = 0;
// Mahesh End
let semiDuty = 0;
let centerDuty = 0;
// Wt adjustments for total stone weight
let semiMinWt = 0;
let centerMinWt = 0;
let SemiAdjWt = 0;
let CenterAdjWt = 0;

let msdCFPLaborData = [];
let msdDutyDetails = [];
let SkuNumberPDF = '';
/// Added By Mahesh Start
var diamondHandling = 0.00;
var diamondHandlingMined = 0.00;
var diaHndLow = 0.00;
var diaHndHigh = 0.00;
var diaHndLowMined = 0.00;
var diaHndHighMined = 0.00;
var findingHndGold = 0.00;
var findingHndPlatinum = 0.00;
var findingHndSilver = 0.00;
var modelMkgGold = 0.00;
var modelMkgPlatinum = 0.00;
var modelMkgSilver = 0.00;
var camGold = 0.00;
var camPlatinum = 0.00;
var camSilver = 0.00;
var semiPrice1Per = 0.0;
var semiPrice2Per = 0.00;
var semiPrice3Per = 0.00;
var semiPrice4Per = 0.00;
var centerPrice1Per = 0.00;
var centerPrice2Per = 0.00;
var centerPrice3Per = 0.00;
var centerPrice4Per = 0.00;
var settingData = false;
//Added Byh Mahesh End
// Load msd JSON once
//fetch('/Config/CFPLaborDetails.json')
//    .then(response => response.json())
//    .then(data => { msdCFPLaborData = data; });

fetch('/Config/DutyDetails.json')
    .then(response => response.json())
    .then(data => { msdDutyDetails = data; });



// Edit index for Findings module
let editIndex = -1;

/**
 * Safely parse a number from input.
 * Removes commas, trims spaces, returns 0 if invalid.
 */
function parseNumber(val) {
    if (val === null || val === undefined) return 0;
    const n = parseFloat(String(val).replace(/,/g, '').trim());
    return isNaN(n) ? 0 : n;
}

/**
 * Debounce helper to prevent rapid API calls.
 */
function debounce(fn, delay) {
    let t;
    return function () {
        const args = arguments;
        clearTimeout(t);
        t = setTimeout(() => fn.apply(this, args), delay);
    };
}
/************************************************************
 *  Script Section 2 — UI INITIALIZATION & BASIC EVENTS
 *  ---------------------------------------------------------
 *  Contains:
 *   - DOM ready initialization
 *   - Active/Deactive toggle button
 *   - Dropdown change handlers
 *   - Table visibility toggling
 ************************************************************/

// Toggle Active / De Active button
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('btnActive').addEventListener('click', function () {
        const button = this;
        if (button.textContent.trim() === 'Active') {
            button.textContent = 'In Active';
        } else {
            button.textContent = 'Active';
        }
    });
});

$(document).ready(function () {

    // Initial table visibility
    toggleTableVisibility();

    // Disable Summary tab until SKU is saved
    if (window.location.href.indexOf("Create") > -1) {
        $('#nav-summary-tab').addClass('disabled').attr('disabled', true);
    }
    //if (window.location.href.indexOf("Info") > -1) {
    //    $('#nav-sku-information-tab').addClass('disabled').attr('disabled', true);
    //    $('#nav-stone-information-tab').addClass('disabled').attr('disabled', true);
    //    $('#nav-labor-information-tab').addClass('disabled').attr('disabled', true);
    //}

    /********************************************************
     * COMPANY DROPDOWN
     ********************************************************/
    $('#ddlCompany').change(function () {
        const selectedValue = $(this).val();
        const selectedText = $('#ddlCompany option:selected').text();
        getMarginDetails();
        console.log('Value:', selectedValue);
    });


    /********************************************************
     * VENDOR DROPDOWN
     ********************************************************/
    $('#ddlVendor').change(function () {
        const selectedValue = $(this).val();
        const selectedText = $('#ddlVendor option:selected').text();
        getTaxDetails('vendor');
        fillLaborFOBValues();
        console.log('Value:', selectedValue);
    });

    /********************************************************
     * FindingSupplier DROPDOWN
     ********************************************************/
    $('#ddlFindingSupplier').change(function () {
        const selectedValue = $(this).val();
        const selectedText = $('#ddlFindingSupplier option:selected').text();
        getTaxDetails('finding', selectedValue);
        fillLaborFOBValues();
        console.log('Value:', selectedValue);
    });


    /********************************************************
     * SotneVendor DROPDOWN
     ********************************************************/
    $('#ddlStoneVendor').change(function () {
        const selectedValue = $(this).val();
        const selectedText = $('#ddlStoneVendor option:selected').text();
        getTaxDetails('diamond', selectedValue);
        fillLaborFOBValues();
        console.log('Value:', selectedValue);
    });


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

    $('#ddlSubCategory').change(function () {
        getMarginDetails();
    });

    /********************************************************
     * METAL DROPDOWN
     * - Sets metal rate
     * - Loads metal colors
     * - Enables/disables karat dropdown
     ********************************************************/
    $('#ddlMetal').change(function () {

        const metalRates = {
            Gold: 5000.00,
            Silver: 100,
            Platinum: 2500
        };
        // Reset All Values first
        $('#txtMetalRatePOz').val('');
        $('#txtMetalGmWt').val('');
        $('#txtMetalRatePerGm').val('');
        $('#txtMetalCost').val('');
        $('#txtMetalInc').val('');
        $('#txtMetalDec').val('');

        const metalType = $(this).val();

        if (metalType && metalRates.hasOwnProperty(metalType)) {
            $('#txtMetalRatePOz').val(metalRates[metalType]);
        } else {
            $('#txtMetalRatePOz').val('');
        }

        const selectedText = $('#ddlMetal option:selected').text();
        const $karatDropdown = $('#ddlKarat');

        // Load metal colors
        $.get(webRoot + '/api/sku/metalcolor?metelType=' + selectedText, function (data) {
            const metelColors = $('#ddlMetalColor');
            metelColors.empty().append('<option value=""></option>');

            $.each(data, function (i, item) {
                metelColors.append($('<option>').val(item.Key).text(item.Value));
            });
        });

        // Enable/disable karat
        if (selectedText.toLowerCase() === 'gold') {
            $karatDropdown.prop('disabled', false).val('');
        } else {
            $karatDropdown.prop('disabled', true).val('');
        }
    });



    ///********************************************************
    // * FINDING SUPPLIER → FINDING TYPE
    // ********************************************************/
    //$('#ddlFindingSupplier').change(function () {
    //    const findingType = $('#ddlFindingType');
    //    const supplier = $(this).val();

    //    if (!supplier) {
    //        findingType.empty().append('<option value=""></option>');
    //    } else {
    //        $.get(webRoot + '/api/sku/findingtype/' + supplier, function (data) {
    //            findingType.empty().append('<option value=""></option>');
    //            $.each(data, function (i, item) {
    //                findingType.append($('<option>').val(item.Key).text(item.Value));
    //            });
    //        });
    //    }
    //});

    /********************************************************
     *  FINDING TYPE -> Metal
     ********************************************************/
    $('#ddlFindingType').change(function () {
        const findingdll = $('#ddlMetalFinding');
        const field = "Metal"
        const filter = $(this).val();

        if (!filter) {
            findingdll.empty().append('<option value=""></option>');
        } else {
            $.get(webRoot + '/api/sku/findingdropdown/' + field + '/' + filter, function (data) {
                findingdll.empty().append('<option value=""></option>');
                $.each(data, function (i, item) {
                    findingdll.append($('<option>').val(item.Key).text(item.Value));
                });
            });
        }
        $('#ddlMetalFinding').change();
    });

    /********************************************************
    *  FINDING TYPE -> Metal -> Karate
    ********************************************************/
    $('#ddlMetalFinding').change(function () {
        const findingdll = $('#ddlKaratFinding');
        const field = "Karat"
        const filter = $('#ddlFindingType').val();
        const filter1 = $(this).val();

        if (filter1 && filter1.toLowerCase() !== 'gold') {

            findingdll.empty().append('<option value=""></option>');
            $('#ddlKaratFinding').change()
        } else {
            $.get(webRoot + '/api/sku/findingdropdown/' + field + '/' + filter + '/' + filter1, function (data) {
                findingdll.empty().append('<option value=""></option>');
                $.each(data, function (i, item) {
                    findingdll.append($('<option>').val(item.Key).text(item.Value));
                });
            });
        }

        // Enable/disable karat
        if (filter1.toLowerCase() === 'gold') {
            findingdll.prop('disabled', false).val(''); $('#ddlKaratFinding').change();
        } else {
            findingdll.prop('disabled', true).val('');
        }
    });

    /********************************************************
    *  FINDING TYPE -> Metal -> Karat -> Color
    ********************************************************/
    $('#ddlKaratFinding').change(function () {
        const findingdll = $('#ddlFindingColor');
        const field = "Color"
        const filter = $('#ddlFindingType').val();
        const filter1 = $('#ddlMetalFinding').val();
        var filter2 = $(this).val();
        if (filter1.toLowerCase() !== 'gold') {
            filter2 = $('#ddlMetalFinding').val();
        }
        if (!filter2 && !filter1) {
            findingdll.empty().append('<option value=""></option>');
        } else {
            $.get(webRoot + '/api/sku/findingdropdown/' + field + '/' + filter + '/' + filter1 + '/' + filter2, function (data) {
                findingdll.empty().append('<option value=""></option>');
                $.each(data, function (i, item) {
                    findingdll.append($('<option>').val(item.Key).text(item.Value));
                });
            });
        }
        $('#ddlFindingColor').trigger("change");
    });

    /********************************************************
    *  FINDING TYPE -> Metal -> Karat -> Color
    ********************************************************/
    $('#ddlFindingColor').change(function () {
        const findingdll = $('#ddlFindingSupplier');
        const field = "Supplier"
        const filter = $('#ddlFindingType').val();
        const filter1 = $('#ddlMetalFinding').val();
        var filter2 = $('#ddlKaratFinding').val();
        const filter3 = $(this).val();
        if (filter1.toLowerCase() !== 'gold') {
            filter2 = $('#ddlMetalFinding').val();
        }
        if (!filter3) {
            findingdll.empty().append('<option value=""></option>');
        } else {
            $.get(webRoot + '/api/sku/findingdropdown/' + field + '/' + filter + '/' + filter1 + '/' + filter2 + '/' + filter3, function (data) {
                findingdll.empty().append('<option value=""></option>');
                $.each(data, function (i, item) {
                    findingdll.append($('<option>').val(item.Key).text(item.Value));
                });
            });
        }
    });


});
//$('#ddlMetalFinding').change(function () {
//    const selectedText = $('#ddlMetalFinding option:selected').text();
//    const $karatDropdown = $('#ddlKaratFinding');

//    // Load metal colors
//    $.get(webRoot + '/api/sku/metalcolor?metelType=' + selectedText, function (data) {
//        const metelColors = $('#ddlFindingColor');
//        metelColors.empty().append('<option value=""></option>');

//        $.each(data, function (i, item) {
//            metelColors.append($('<option>').val(item.Key).text(item.Value));
//        });
//    });

//    // Enable/disable karat
//    if (selectedText.toLowerCase() === 'gold') {
//        $karatDropdown.prop('disabled', false).val('');
//    } else {
//        $karatDropdown.prop('disabled', true).val('');
//    }

//});
/************************************************************
 *  Script Section 3 — AUTOCOMPLETE (Finding Description & SKU)
 *  ---------------------------------------------------------
 *  Contains:
 *   - jQuery UI autocomplete for Finding Description & SKU
 *   - Shared initializer for both textboxes
 *   - API mapping for autocomplete results
 *   - Selection behavior (fills both fields)
 *   - loadFindingDetails() for auto-populating fields
 ************************************************************/

$(document).ready(function () {

    /********************************************************
     * AUTOCOMPLETE: Finding Search (Legacy Single Box)
     ********************************************************/
    $("#txtFindingSearch").autocomplete({
        minLength: 5,
        delay: 300,
        source: function (request, response) {
            const findingSupplier = $('#ddlFindingSupplier').val();
            const findingType = $('#ddlFindingType').val();

            $.ajax({
                url: webRoot + '/api/sku/findingdescription/' +
                    findingSupplier + '/' + findingType + '/' + request.term,
                type: 'GET',
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.description + ' (' + item.sku + ')',
                            value: item.description,
                            id: item.findingId,
                            sku: item.sku
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $("#txtFindingSearch").val(ui.item.value);
            $("#hdnFindingId").val(ui.item.id);
            $("#txtSku").val(ui.item.sku);
            return false;
        }
    });

    /********************************************************
     * SHARED AUTOCOMPLETE INITIALIZER
     * mode = 'desc' → user types description
     * mode = 'sku'  → user types SKU
     ********************************************************/
    function initFindingAutocomplete($sourceBox, $targetBox, mode) {

        $sourceBox.autocomplete({
            minLength: 5,
            delay: 100,
            source: function (request, response) {

                const findingSupplier = $('#ddlFindingSupplier').val() || '';
                const findingType = $('#ddlFindingType').val() || '';
                const search = request.term;

                const field = "Supplier";
                const filter = $('#ddlFindingType').val();
                const filter1 = $('#ddlMetalFinding').val();
                var filter2 = $('#ddlKaratFinding').val();
                /* const filter3 = request.term;*/
                if (!findingSupplier || !findingType) {
                    response([]);
                    return;
                }

                const url = webRoot + '/api/sku/findingdescription/' +
                    encodeURIComponent(findingSupplier) + '/' +
                    encodeURIComponent(findingType) + '/' +
                    encodeURIComponent(search);

                $.getJSON(url, function (data) {
                    console.log(data);
                    response($.map(data, function (item) {
                        //return {
                        //    label: item.Key,
                        //    value: item.Value
                        //    };

                        if (mode === 'desc') {
                            return {
                                label: item.Key,
                                value: item.Value
                            };
                        } else {
                            return {
                                label: item.Value,
                                value: item.Key
                            };
                        }
                    }));
                });
            },

            select: function (event, ui) {
                if (mode === 'desc') {
                    $sourceBox.val(ui.item.label);
                    $targetBox.val(ui.item.value);
                    loadFindingDetails(ui.item.value);
                } else {
                    $sourceBox.val(ui.item.label);
                    $targetBox.val(ui.item.value);
                    loadFindingDetails(ui.item.value);
                }
                return false;
            }
        });
    }

    // Initialize autocomplete for both textboxes
    initFindingAutocomplete($('#txtFindingDescription'), $('#txtFindingSku'), 'desc');
    initFindingAutocomplete($('#txtFindingSku'), $('#txtFindingDescription'), 'sku');

    var availableTags = [
        "ActionScript",
        "AppleScript",
        "Asp",
        "BASIC",
        "C",
        "C++",
        "Clojure",
        "COBOL"
    ];
    $("#txtFindingDescription1").autocomplete({
        source: availableTags
    });


});

/************************************************************
 * LOAD FINDING DETAILS (Auto-fill fields)
 ************************************************************/
function loadFindingDetails(sku) {
    if (!sku) return;

    const url = webRoot + '/api/sku/findingdetails/' + encodeURIComponent(sku);

    $.getJSON(url, function (data) {
        if (!data) return;

        //$('#txtMetal').val(data.findingMetalType || data.FindingMetalType);
        // $('#ddlMetalFinding').val(data.findingMetalType || data.FindingMetalType);
        //// $('#txtKarat').val(data.findingMetalKt || data.FindingMetalKt);
        // $('#ddlKaratFinding').val(data.findingMetalKt || data.FindingMetalKt);

        // $('#txtColor').val(data.findingMetalColor || data.FindingMetalColor);
        $('#txtFindingDescription').val(data.FindingDescription || data.FindingDescription);
        $('#txtFindingGmPerPc').val(data.perPcFindingWeightGms || data.PerPcFindingWeightGms);
        $('#txtFindingInc').val(data.increment || data.Increment);
        $('#txtFindingDec').val(data.decrement || data.Decrement);
        $('#txtFindingCostPerPc').val(data.findingCost || data.FindingCost);
    });
}



/************************************************************
 * LOAD VENDOR DETAILS (Auto-fill fields)
 ************************************************************/
function getVendorDetails(VendorCode) {
    if (!VendorCode) return;

    const url = webRoot + '/api/sku/vendordetails/' + encodeURIComponent(VendorCode);

    $.getJSON(url, function (data) {
        if (!data) return;
        
        diamondHandlingMined = data.DiamondHandlingMined || 0;
            diaHndLowMined = data.DiaHndMinedLow || 0;
            diaHndHighMined = data.DiaHndMinedHigh || 0;
       
            diamondHandling = data.DiamondHandlingLab || 0;
            diaHndLow = data.DiaHndLabLow || 0;
            diaHndHigh = data.DiaHndLabHigh || 0;
        
        //diaHndLow = data.DiaHndLabLow || 0;
        //diaHndHigh = data.DiaHndLabHigh || 0;

        findingHndGold = data.FindingHndGold || 0;
        findingHndPlatinum = data.FindingHndPlatinum || 0;
        findingHndSilver = data.FindingHndSilver || 0;

        modelMkgGold = data.ModelMkgGold || 0;
        modelMkgPlatinum = data.ModelMkgPlatinum || 0;
        modelMkgSilver = data.ModelMkgSilver || 0;

        camGold = data.CAMGold || 0;
        camPlatinum = data.CAMPlatinum || 0;
        camSilver = data.CAMSilver || 0;

        HandlingCaluculation();

    });
}

function HandlingCaluculation() {

    var DimondHndl = 0.0;
    // Dimond Handling Calculation Start

    var totalStnCost = stoneList.reduce(function (sum, item) {
        return sum + (parseFloat(item.StoneTotalCost) || 0);
    }, 0);

    var totalDHAdjWT = stoneList.reduce(function (sum, item) {
        return sum + (parseFloat(item.TotalAdjStoneWt) || 0);
    }, 0);
    var dimondhndlSTCost = (parseFloat(totalStnCost).toFixed(2) * parseFloat(diamondHandling).toFixed(2)) / 100;
    var dimondhndlAadjwtCost = parseFloat(totalDHAdjWT).toFixed(3) * parseFloat(diaHndLow).toFixed(2);
    //DimondHndl = Math.min(dimondhndlSTCost, dimondhndlAadjwtCost);
    DimondHndl =  Math.min(parseFloat(dimondhndlSTCost), parseFloat(dimondhndlAadjwtCost));
    if (DimondHndl > diaHndHigh) {
        DimondHndl = diaHndHigh;
    }
    //if (DimondHndl < diaHndLow) {
    //    DimondHndl = diaHndLow;
    //}
    $("#txtDiaHandling").val(parseFloat(DimondHndl).toFixed(2)).trigger("change");

    // Dimond Handling Calculation End
    var labourFindingcost = 0.0;
    var vendorFindingCost = 0.0;
    var vendorModelCost = 0.00;
    var vendorCamCost = 0.00;
    var metalType1 = 'gold';
    var distinctMetal = new Set(findingLines.map(x => x.metal));

    //var totalStnCost = findingLines.reduce(function (sum, item) {
    //    return sum + (parseFloat(item.totalCost) || 0);
    //}, 0);
    distinctMetal.forEach(matelType => {
        var findingtotalCost = findingLines.filter(x => x.metal.toLowerCase() === matelType.toLowerCase())
            .reduce(function (sum, item) {
                return sum + (parseFloat(item.totalCost) || 0);
            }, 0);
        if (matelType.toLowerCase() === 'gold') {
            vendorFindingCost = findingHndGold;
        } else if (matelType.toLowerCase() === 'platinum') {
            vendorFindingCost = findingHndPlatinum;
        } else if (matelType.toLowerCase() === 'silver') {
            vendorFindingCost = findingHndSilver;
        }
        labourFindingcost += (parseFloat(findingtotalCost) * parseFloat(vendorFindingCost)) / 100;
    });

    $('#txtFinHandling').val(parseFloat(labourFindingcost).toFixed(2));
   

    if (metalLines.length > 0) {
        metalType1 = metalLines[0].metalText;
    }
    var ordertype = $('#ddlOrderType').val();
    if (ordertype.toLowerCase() === 'special') { 
    if (metalType1.toLowerCase() === 'gold') {
        vendorModelCost = modelMkgGold;
        vendorCamCost = camGold;
    } else if (metalType1.toLowerCase() === 'platinum') {
        vendorModelCost = modelMkgPlatinum;
        vendorCamCost = camPlatinum;
    } else if (metalType1.toLowerCase() === 'silver') {
        vendorModelCost = modelMkgSilver;
        vendorCamCost = camSilver;

        }
    }
    $('#txtModel').val(parseFloat(vendorModelCost).toFixed(2));
    $('#txtCAM').val(parseFloat(vendorCamCost).toFixed(2));
    $('#txtCAM').trigger("change");
    calculateTotalLabor();

}

// Trigger loadFindingDetails when SKU changes manually
$('#txtFindingSku').on('change', function () {
    loadFindingDetails($(this).val().trim());
});

$('#txtSemiMinWt').on('input', function () {
    //$('#txtSemiAdjWt').trigger('change');
    validateSemiAdj();
    calculateTotals();
}); 
$('#txtCenterMinWt').on('input', function () {
    validateCenterAdj();
    calculateTotals();
}); 

$('#ddlLaborLocation').on('change', function () {
    //var vendor = encodeURIComponent($('#ddlLaborLocation  option:selected').text().trim());
    //var vendor = encodeURIComponent($('#ddlLaborLocation').val());
    var vendorlist = $('#ddlLaborLocation').val().split('|');
    var vendor = encodeURIComponent(vendorlist[0]);
    var type = encodeURIComponent($(this).val().trim());
    var category = encodeURIComponent($('#ddlCategory  option:selected').text().trim());
    var url = webRoot + '/api/sku/otherOptional?vendorcode=' + vendor + '&category=' + category;
    $.get(url, function (data) {
        const subDropdown1 = $('#ddlOtherHead1');
        const subDropdown2 = $('#ddlOtherHead2');
        const subDropdown3 = $('#ddlOtherHead3');
        subDropdown1.empty().append('<option value=""></option>');
        subDropdown2.empty().append('<option value=""></option>');
        subDropdown3.empty().append('<option value=""></option>');

        $.each(data, function (i, item) {
            subDropdown1.append($('<option>').val(item.Key).text(item.Value));
            subDropdown2.append($('<option>').val(item.Key).text(item.Value));
            subDropdown3.append($('<option>').val(item.Key).text(item.Value));
        });
    });
    $("#ddlProcessType").trigger("change");
    const selectedValue = vendorlist[0];
    const selectedText = $('#ddlLaborLocation option:selected').text();
    getTaxDetails('labor', selectedValue);
    
    console.log('labor:', selectedValue);
    
});


$('#ddlProcessType').on('change', function () {
    //var vendor = encodeURIComponent($('#ddlLaborLocation  option:selected').text().trim());
    var vendorlist = $('#ddlLaborLocation').val().split('|');
    var vendor = encodeURIComponent(vendorlist[0]);
    var type = encodeURIComponent($(this).val().trim());
    var category = encodeURIComponent($('#ddlCategory  option:selected').text().trim());
    if (type === 'Flat Labour per piece') {
        txtboxDisEna('disabled');
        $('#txtTotalLabor').val(0.001);
        $('#txtTotalLabor').prop('disabled', false).prop('readonly', false);
        return;
    }

    var url = webRoot + '/api/sku/getprocesscost/' + vendor + '/' + type + '/' + category;
    $.getJSON(url, function (data) {
        if (!data) {
            data = {
                GoldCharges: 0,
                PlatinumCharges: 0,
                SilverCharges:0,
            }

        }

        setProcessValues(data, 'txtCFP', type);
    //    if (type === 'CFP') {
    //        var vendor = encodeURIComponent($('#ddlLaborLocation').val());
            
    //    } 
    });

    if (encodeURIComponent($(this).val().trim()) === 'CFP' && metalLines[0].metalText === 'Gold' && metalLines[0].colorText === 'White') {
        type = 'Rhodium';
        url = webRoot + '/api/sku/getprocesscost/' + vendor + '/Rhodium/' + category;
        $.getJSON(url, function (data) {
            if (!data) {
                data = {
                    GoldCharges: 0,
                    PlatinumCharges: 0,
                    SilverCharges: 0,
                }

            }
            setProcessValues(data, 'txtRhodium', type);
        });
        var ptype = encodeURIComponent('Laser Stamping');
        type = 'Stamping';
        url = webRoot + '/api/sku/getprocesscost/' + vendor + '/' + ptype +'/' + category;
        $.getJSON(url, function (data) {
            if (!data) {
                data = {
                    GoldCharges: 0,
                    PlatinumCharges: 0,
                    SilverCharges: 0,
                }

            }
            setProcessValues(data, 'txtStamping', type);
        });
    }

    //calculateTotalLabor();
});

function setProcessValues(data, ctrl, type) {
    type = decodeURI(type);
    if (type === 'CFP' || type === 'Rhodium' || type === 'Stamping') {
        if (metalLines[0].metalText === 'Gold') {
            $('#' + ctrl).val(data.GoldCharges);
        }
        else if (metalLines[0].metalText === 'Platinum') {
            $('#' + ctrl).val(data.PlatinumCharges);
        }
        else if (metalLines[0].metalText === 'Silver') {
            $('#' + ctrl).val(data.SilverCharges);
        }
        //HandlingCaluculation();
       // var vendor = encodeURIComponent($('#ddlLaborLocation').val());
        var vendorlist = $('#ddlLaborLocation').val().split('|');
       var vendor = encodeURIComponent(vendorlist[0]);
        if (data.Type === 'CFP') {
            getVendorDetails(vendor);
        }
        calculateTotalLabor();
        //fillLaborFOBValues();

        txtboxDisEna('enabled');
        $('#txtTotalLabor').prop('disabled', true).prop('readonly', true);

    } else if (type === 'Flat Labour per gm') {
        const semiMin = parseFloat(txtSemiMinWt.value) || 0;
        // const semiAdj = parseFloat(txtSemiAdjWt.value) || 0;
        const centerMin = parseFloat(txtCenterMinWt.value) || 0;
        //const centerAdj = parseFloat(txtCenterAdjWt.value) || 0;

        // txtTotalMinWt.value = (semiMin + centerMin).toFixed(3); // keep 4 decimals
        var totalwt = (semiMin + centerMin).toFixed(3);
        totalwt = 0;
        if (metalLines.length > 0) {
            totalwt = metalLines[0].gmWt;
        }

        var cost = 0;
        if (metalLines[0].metalText === 'Gold') {
            cost = data.GoldCharges;
        }
        else if (metalLines[0].metalText === 'Platinum') {
            cost = data.PlatinumCharges;
        }
        else if (metalLines[0].metalText === 'Silver') {
            cost = data.SilverCharges;
        }
        var charges = (parseFloat(totalwt) * cost).toFixed(2);
        $('#txtTotalLabor').val(charges);
        calculateTotalLabor();

        //fillLaborFOBValues();
        txtboxDisEna('disabled');
        $('#txtTotalLabor').prop('disabled', true).prop('readonly', true);
        //calculateTotalLabor();
    } else if (type === 'Flat Labour per piece') {
        const semiMin = parseFloat(txtSemiMinWt.value) || 0;

        const centerMin = parseFloat(txtCenterMinWt.value) || 0;
        var costSilver = 0.00;
        var costPlatinum = 0.00;
        cost = data.GoldCharges;
        costPlatinum = data.PlatinumCharges;
        costSilver = data.SilverCharges;


        //if (metalLines[0].metalText === 'Gold') {
        //    cost = data.GoldCharges;
        //}
        //else if (metalLines[0].metalText === 'Platinum') {
        //    cost = data.PlatinumCharges;
        //}
        //else if (metalLines[0].metalText === 'Silver') {
        //    cost = data.SilverCharges;
        //}

        var totalGoldRate = metalLines.filter(s => s.metalText.toLowerCase() === "gold")
            .reduce((sum, s) => sum + (parseFloat(s.metalCost) || 0), 0);

        var totalPlatinumRate = metalLines.filter(s => s.metalText.toLowerCase() === "platinum")
            .reduce((sum, s) => sum + (parseFloat(s.metalCost) || 0), 0);
        var totalSilverRate = metalLines.filter(s => s.metalText.toLowerCase() === "silver")
            .reduce((sum, s) => sum + (parseFloat(s.metalCost) || 0), 0);

        var chargesGold = (parseFloat(totalGoldRate) * cost).toFixed(2);
        var chargesPlatinum = (parseFloat(totalPlatinumRate) * costPlatinum).toFixed(2);
        var chargesSilver = (parseFloat(totalSilverRate) * costSilver).toFixed(2);
        $('#txtTotalLabor').val(parseFloat(chargesGold) + parseFloat(chargesPlatinum) + parseFloat(chargesSilver));
        calculateTotalLabor();

        //fillLaborFOBValues();
        txtboxDisEna('disabled');
        $('#txtTotalLabor').prop('disabled', false).prop('readonly', false);
        //$('#txtTotalLabor').val('');
        //calculateTotalLabor();
    }
    else if (ctrl.includes('ddlOtherHead')) {

        ctrl = ctrl.replace('ddlOtherHead', 'txtOtherCost');
        if (metalLines[0].metalText === 'Gold') {
            $('#' + ctrl).val(data.GoldCharges);
        }
        else if (metalLines[0].metalText === 'Platinum') {
            $('#' + ctrl).val(data.PlatinumCharges);
        }
        else if (metalLines[0].metalText === 'Silver') {
            $('#' + ctrl).val(data.SilverCharges);
        }
        calculateTotalLabor();
        //fillLaborFOBValues();
    }

}

["ddlOtherHead1", "ddlOtherHead2", "ddlOtherHead3"].forEach(id => {
    const el = document.getElementById(id);
    if (el) {
        el.addEventListener("change", function () {
            var ctrl = this.id
            var vendorlist = $('#ddlLaborLocation').val().split('|');
            var vendor = encodeURIComponent(vendorlist[0]);
            // var vendor = encodeURIComponent($('#ddlLaborLocation  option:selected').text().trim());
            var type = encodeURIComponent($(this).val().trim());
            // var category = encodeURIComponent($('#' + ctrl + '  option:selected').text().trim());
            var category = encodeURIComponent($('#ddlCategory  option:selected').text().trim());

            var url = webRoot + '/api/sku/getprocesscost/' + vendor + '/' + type + '/' + category;
            if (category.trim().length > 0) {
                $.getJSON(url, function (data) {
                    if (!data) return;

                    setProcessValues(data, ctrl, type);
                });
            } else {
                ctrl = ctrl.replace('ddlOtherHead', 'txtOtherCost');
                $('#' + ctrl).val('');
                calculateTotalLabor();
            }
        });
    }
});

function txtboxDisEna(status) {
    var setField = ["txtRhodium", "txtDiaHandling", "txtFinHandling", "txtStamping", "txtModel", "txtCAM", "txtGiftBox", 'txtCFP'];

    setField.forEach(id => {
        var $txt = $('#' + id);
        if ($txt) {

            if (status === 'disabled') {
                $txt.prop('disabled', true).prop('readonly', true);
                $txt.val('');
            }
            else {
                $txt.prop('disabled', false).prop('readonly', false);
            }
        }
    });

}

/************************************************************
 *  Script Section 4 — FINDINGS MODULE (CRUD + TABLE RENDERING)
 *  ---------------------------------------------------------
 *  Contains:
 *   - readFindingForm()
 *   - fillForm()
 *   - clearForm()
 *   - renderTable()
 *   - Add / Update / Delete events
 *   - Finding totals recalculation
 ************************************************************/

/**
 * Read all Finding form fields and compute totals.
 */
function readFindingForm() {
    const qty = parseFloat($('#txtFindingQty').val()) || 0;
    const gmPerPc = parseFloat($('#txtFindingGmPerPc').val()) || 0;
    const costPer = parseFloat($('#txtFindingCostPerPc').val()) || 0;

    const totalGm = qty * gmPerPc;
    const totalCost = qty * costPer;
    // Total Finding Cost accumulator
    totalFindingCost += totalCost;
    $('#txtFindingTotalGmWt').val(totalGm.toFixed(3));
    $('#txtFindingTotalCost').val(totalCost.toFixed(2));
    findingDutyVal = totalCost * findingDutyper;
    findingPenaltyVal = totalCost * findingPenaltyPer;
    findingTariffVal = totalCost * findingTariffPer;
    return {
        supplier: $('#ddlFindingSupplier option:selected').text(),
        supplierId: $('#ddlFindingSupplier').val(),
        type: $('#ddlFindingType option:selected').text(),
        typeId: $('#ddlFindingType').val(),
        description: $('#txtFindingDescription').val(),
        sku: $('#txtFindingSku').val(),
        //metal: $('#txtMetal').val(),
        metal: $('#ddlMetalFinding').val(),
        //karat: $('#txtKarat').val(),
        karat: $('#ddlKaratFinding').val(),
        karattext: $('#ddlKaratFinding option:selected').text(),
        //color: $('#txtColor').val(),
        color: $('#ddlFindingColor').val(),
        assembly: $('#ddlFindingAssembly option:selected').text(),
        assemblyId: $('#ddlFindingAssembly').val(),
        qty: qty,
        gmWtPerPc: gmPerPc,
        totalGm: totalGm,
        costPerPc: costPer,
        totalCost: totalCost,
        inc: $('#txtFindingInc').val(),
        dec: $('#txtFindingDec').val(),
        findingDutyVal: findingDutyVal,
        findingTariffVal:findingTariffVal,
        findingPenaltyVal:findingPenaltyVal
    };
}

/**
 * Fill form fields when editing a row.
 */
function fillForm(row) {

    $('#ddlFindingType').val(row.typeId).change();
    setTimeout(function () {
        $('#ddlMetalFinding').val(row.metal).change();
        setTimeout(function () {
            $('#ddlKaratFinding').val(row.karat).change();
            setTimeout(function () {
                $('#ddlFindingColor').val(row.color).change();
                setTimeout(function () {
                    $('#ddlFindingSupplier').val(row.supplierId).change();

                    $('#txtFindingDescription').val(row.description);
                    $('#txtFindingSku').val(row.sku);
                    $('#ddlFindingAssembly').val(row.assemblyId);
                    $('#txtFindingQty').val(row.qty);
                    $('#txtFindingGmPerPc').val(row.gmWtPerPc);
                    $('#txtFindingTotalGmWt').val(row.totalGm.toFixed(3));
                    $('#txtFindingCostPerPc').val(row.costPerPc);
                    $('#txtFindingTotalCost').val(row.totalCost.toFixed(2));
                    $('#txtFindingInc').val(row.inc);
                    $('#txtFindingDec').val(row.dec);
                }, 800);
            }, 800);
        }, 800);
    }, 800);





    //$('#txtMetal').val(row.metal);

    //$('#txtKarat').val(row.karat);




}

/**
 * Clear Finding form fields.
 */
function clearForm() {
    editIndex = -1;
    $('#btnFindingAddUpdate').text('Add Finding');

    $('#txtFindingDescription,#ddlFindingType,#txtFindingSku,#ddlFindingColor,#ddlFindingSupplier, #ddlMetalFinding, #txtMetal, #ddlKaratFinding, #txtColor,' +
        '#txtFindingQty, #txtFindingGmPerPc, #txtFindingTotalGmWt,' +
        '#txtFindingCostPerPc, #txtFindingTotalCost,' +
        '#txtFindingInc, #txtFindingDec').val('');

    $('#ddlFindingAssembly').val('');
}

/**
 * Render Findings table.
 */
function renderTable() {
    const $tbody = $('#tblFindings tbody');
    $tbody.empty();
    var karat = '';
    findingtotaltax = 0.00;
    findingLines.forEach((row, idx) => {
        if (row.metal == 'Gold') {
            karat = row.karatText || row.karat + 'K';
        }
        findingtotaltax +=(row.findingDutyVal + row.findingTariffVal + row.findingPenaltyVal)
        const tr = `
            <tr data-index="${idx}">
                <td>${row.type}</td>
                <td>${karat + ' ' + row.color + ' ' + row.metal}</td>
                <td>${row.supplier}</td>
                <td>${row.sku}</td>
                <td style="text-align:left;">${row.description}</td>
                <td>${row.qty}</td>
                <td>${row.gmWtPerPc}</td>
                <td>${row.totalGm.toFixed(3)}</td>
                <td>$ ${row.costPerPc}</td>
                <td>$ ${row.totalCost.toFixed(2)}</td>
                <td>${row.assembly}</td>

                <!-- hidden inc/dec -->
                <td style="display:none;">${row.inc}</td>
                <td style="display:none;">${row.dec}</td>

                <td class="text-center">
                    <button class="btn btn-sm btn-warning btn-edit me-1">Edit</button>
                    <button class="btn btn-sm btn-danger btn-delete">Delete</button>
                </td>
            </tr>`;
        $tbody.append(tr);
    });
}

/************************************************************
 * ADD / UPDATE / DELETE EVENTS
 ************************************************************/
$(function () {

    // Add or Update Finding
    $('#btnFindingAddUpdate').on('click', function (e) {
        e.preventDefault();
        if (validateButtonAddUpdate("btnFindingAddUpdate")) {
            if (editIndex > -1) {
                totalFindingCost = parseFloat(totalFindingCost) - parseFloat(findingLines[editIndex].totalCost);
            }
            const row = readFindingForm();

            if (editIndex === -1) {
                findingLines.push(row);
            } else {
                findingLines[editIndex] = row;
            }

            renderTable();
            toggleTableVisibility();
            clearForm();
        }
    });

    // Edit Finding
    $('#tblFindings').on('click', '.btn-edit', function () {
        const idx = $(this).closest('tr').data('index');
        editIndex = idx;
        fillForm(findingLines[idx]);
        $('#btnFindingAddUpdate').text('Update Finding');
    });

    // Delete Finding
    $('#tblFindings').on('click', '.btn-delete', function () {
        const idx = $(this).closest('tr').data('index');
        totalFindingCost = parseFloat(totalFindingCost) - parseFloat(findingLines[idx].totalCost);
        findingLines.splice(idx, 1);
        renderTable();
        toggleTableVisibility();
        clearForm();
    });

});

/************************************************************
 * FINDING TOTALS RECALCULATION
 ************************************************************/
function recalcFindingTotals() {
    const qty = parseFloat($('#txtFindingQty').val()) || 0;
    const gmPerPc = parseFloat($('#txtFindingGmPerPc').val()) || 0;
    const costPer = parseFloat($('#txtFindingCostPerPc').val()) || 0;
    const rateOz = parseFloat($('#txtMetalRatePOz').val()) || 0;

    const totalGm = qty * gmPerPc;
    const totalCost = qty * costPer;
    const findingDec = totalCost / rateOz;
    const findingInc = findingDec / 0.65;

    $('#txtFindingDec').val(findingDec.toFixed(3));
    $('#txtFindingInc').val(findingInc.toFixed(3));
    $('#txtFindingTotalGmWt').val(totalGm.toFixed(2));
    $('#txtFindingTotalCost').val(totalCost.toFixed(2));
}

// Bind recalculation events
$('#txtFindingQty').on('input', recalcFindingTotals);
$('#txtFindingGmPerPc, #txtFindingCostPerPc').on('input', recalcFindingTotals);
/************************************************************
 *  Script Section 5 — METALS MODULE (CRUD + TABLE RENDERING)
 *  ---------------------------------------------------------
 *  Contains:
 *   - readMetalForm()
 *   - renderMetalGrid()
 *   - Add/Delete events
 *   - Metal cost calculation
 *   - Rate per gram calculation
 ************************************************************/

/**
 * Read all Metal form fields and compute cost.
 */
function readMetalForm() {
    const gmWt = parseFloat($('#txtMetalGmWt').val()) || 0;
    const ratePOz = parseFloat($('#txtMetalRatePOz').val()) || 0;
    const rateGm = parseFloat($('#txtMetalRatePerGm').val()) || 0;
    const cost = parseFloat($('#txtMetalCost').val()) || 0;

    // Total Metal Cost accumulator
    //totalMetalCost += cost;
    vendorDutyVal = cost * vendorDutyper;
    vendorPenaltyVal = cost * vendorPenaltyPer;
    vendorTariffVal = cost * vendorTariffPer;
    return {
        metalText: $('#ddlMetal option:selected').text(),
        metalId: $('#ddlMetal').val(),
        karatText: $('#ddlKarat option:selected').text(),
        karatId: $('#ddlKarat').val(),
        colorText: $('#ddlMetalColor option:selected').text(),
        colorId: $('#ddlMetalColor').val(),

        Metal: $('#ddlMetal option:selected').text(),
        Karat: $('#ddlKarat option:selected').text(),
        Color: $('#ddlMetalColor option:selected').text(),

        gmWt: gmWt,
        ratePOz: ratePOz,
        ratePerGm: rateGm,
        RateInGrams: rateGm,
        metalCost: cost,

        inc: $('#txtMetalInc').val(),
        dec: $('#txtMetalDec').val(),
        metalDutyVal: vendorDutyVal,
        metalPenaltyVal: vendorPenaltyVal,
        metalTariffVal:vendorTariffVal
        
    };
}

/**
 * Render Metals table.
 */
function renderMetalGrid() {
    const $tbody = $('#tblMetals tbody');
    $tbody.empty();
    totalMetalCost = 0;
    vendortotaltax = 0;
    metalLines.forEach((row, idx) => {
        totalMetalCost += row.metalCost;
        vendortotaltax += row.metalDutyVal + row.metalPenaltyVal + row.metalTariffVal;
        const tr = `
            <tr data-index="${idx}">
                <td class="text-center">${row.metalText}</td>
                <td class="text-center">${row.karatText}</td>
                <td class="text-center">${row.colorText}</td>
                <td class="text-center">${row.gmWt}</td>
                <td class="text-center">$ ${row.ratePerGm}</td>
                <td class="text-center">$ ${row.metalCost}</td>

                <!-- hidden inc/dec -->
                <td style="display:none;">${row.inc}</td>
                <td style="display:none;">${row.dec}</td>

                <td class="ms-autocust text-center">
                    <button class="btn btn-sm btn-danger btn-delete-metal">Delete</button>
                </td>
            </tr>`;
        $tbody.append(tr);
    });
}

/************************************************************
 * ADD / DELETE EVENTS
 ************************************************************/
$(function () {

    // Add Metal
    $('#btnMetalAddUpdate').on('click', function () {
        if (validateButtonAddUpdate("btnMetalAddUpdate")) {
            const row = readMetalForm();
            metalLines.push(row);
            $('#ddlMetal').val('');
            $('#txtMetalRatePOz').val('0.00');
            $('#ddlKarat').val('');
            $('#ddlMetalColor').val('');
            $('#txtMetalGmWt').val('0');
            $('#txtMetalRatePerGm').val('0.00');
            $('#txtMetalCost').val('0.00');
            renderMetalGrid();
            toggleTableVisibility();
        }
    });

    // Delete Metal
    $('#tblMetals').on('click', '.btn-delete-metal', function () {
        const idx = $(this).closest('tr').data('index');
        var cost = parseFloat(metalLines[idx].metalCost);

        totalMetalCost = parseFloat(totalMetalCost) - parseFloat(cost);
        metalLines.splice(idx, 1);

        renderMetalGrid();
        toggleTableVisibility();
    });

});

/************************************************************
 * METAL COST CALCULATION
 ************************************************************/
function calculateMetalCost() {

    const gmWt = parseFloat($('#txtMetalGmWt').val()) || 0;
    const rateOz = parseFloat($('#txtMetalRatePOz').val()) || 0;
    const metal = $('#ddlMetal').val();
    const karat = parseFloat($('#ddlKarat').val()) || 24;
    const vendor = $('#ddlVendor').val().trim();
    calculateRatePerGram(metal, rateOz, karat, vendor)
        .then(rate => {
            if (rate) {
                console.log("Rate per gram:", rate);
                $("#txtMetalRatePerGm").val(rate);
            } else {
                console.log("Invalid inputs");
            }
        })
        .catch(err => console.error("Error fetching metal loss:", err));
    const ratePerGm = parseFloat($('#txtMetalRatePerGm').val()) || 0;
    const metalCost = ratePerGm * gmWt;
    const metalDec = metalCost / rateOz;
    const metalInc = metalDec / 0.65;

    $('#txtMetalCost').val(metalCost.toFixed(2));
    $('#txtMetalDec').val(metalDec.toFixed(3));
    $('#txtMetalInc').val(metalInc.toFixed(3));



    calculateRatePerGram(metal, rateOz, karat, vendor)
        .then(rate => {
            if (rate) {
                console.log("Rate per gram:", rate);
                $("#txtMetalRatePerGm").val(rate);
            } else {
                console.log("Invalid inputs");
            }
        })
        .catch(err => console.error("Error fetching metal loss:", err));
}

/************************************************************
 * RATE PER GRAM CALCULATION
 ************************************************************/
//function calculateRatePerGram() {
//    const metal = $('#ddlMetal').val();
//    const rateOz = parseFloat($('#txtMetalRatePOz').val()) || 0;
//    const karat = parseFloat($('#ddlKarat').val()) || 24;
//    const vendor = $('#ddlVendor option:selected').text().trim();

//    if (!metal || !vendor || !rateOz) {
//        $('#txtMetalRatePerGm').val('');
//        return;
//    }

//    const url = webRoot + '/api/sku/metalloss/' +
//        encodeURIComponent(vendor) + '/' +
//        encodeURIComponent(metal);

//    $.getJSON(url, function (data) {
//        const first = (data && data.length) ? data[0] : null;
//        const metalLossPercent = first ? parseFloat(first.Value) || 0 : 0;

//        const lossFactor = 1 + (metalLossPercent / 100.0);

//        const ratePerGram =
//            rateOz * karat / 31.1035 / 24 * lossFactor;

//        $('#txtMetalRatePerGm').val(ratePerGram.toFixed(2));
//    });
//}


function calculateRatePerGram(metal, rateOz, karat = 24, vendor) {
    return new Promise((resolve, reject) => {
        if (!metal || !vendor || !rateOz) {
            resolve(null); // return null if inputs are invalid
            return;
        }

        const url = webRoot + '/api/sku/metalloss/' +
            encodeURIComponent(vendor) + '/' +
            encodeURIComponent(metal);

        $.getJSON(url, function (data) {
            const first = (data && data.length) ? data[0] : null;
            const metalLossPercent = first ? parseFloat(first.Value) || 0 : 0;

            const lossFactor = 1 + (metalLossPercent / 100.0);

            const ratePerGram = rateOz * karat / 31.1035 / 24 * lossFactor;

            resolve(ratePerGram.toFixed(2)); // return as string with 2 decimals
        }).fail(err => reject(err));
    });
}



// Bind metal cost calculation
$('#txtMetalGmWt').on('input', calculateMetalCost);
$('#txtMetalGmWt').on('blur', calculateMetalCost);
/************************************************************
 *  Script Section 6 — STONE MODULE
 *  ---------------------------------------------------------
 *  Contains:
 *   - ELEMENT REFERENCES
 *   - MM SIZE → PER STONE WEIGHT LOADER
 *   - STONE COST PER CARAT LOADER
 *   - SETTING VENDOR → SETTING TYPE LOADER
 *   - SETTING COST PER STONE LOADER
 *   - TOTAL COST CALCULATOR
 *   - Stone Type → Stone Shape
 *   - Stone Shape → Stone Quality
 *   - Setting Location logic
 *   - Per-stone weight API
 *   - Total stone weight calculation
 ************************************************************/

// ======================================================
//  ELEMENT REFERENCES
// ======================================================
const $mmSize = $('#txtStoneMMSize');
const $stoneType = $('#ddlStoneType');
const $growingType = $('#ddlGrowing');
const $stoneShape = $('#ddlStoneShape');
const $stoneVendor = $('#ddlStoneVendor');
const $ddlQuality = $('#ddlStoneQuality');
const $ddlStoneType = $('#ddlStoneType');
const $ddlGrowingType = $('#ddlGrowingType');
const $ddlStoneShape = $('#ddlStoneShape');
const $ddlSizeRange = $('#ddlSizeRange');

const $txtCost = $('#txtStoneCostPerCarat');
const $txtStoneTotalCost = $('#txtStoneTotalCost');
const $txtTotalAdjStoneWt = $('#txtTotalAdjStoneWt');
const $txtTotalStoneWt = $('#txtTotalStoneWt'); // referenced but missing earlier

const $costPerStone = $('#txtCostPerStone');
const $stoneQty = $('#txtStoneQty');
const $totalCost = $('#txtTotalCost');
const $txtPerStoneWt = $('#txtPerStoneWt');


// ======================================================
//  MM SIZE → PER STONE WEIGHT LOADER
// ======================================================
function onMmSizeChange() {
    const lengthDiameter = $mmSize.val()?.toString().trim();
    if (!lengthDiameter) {
        $('#txtPerStoneWt').val('');
        return;
    }

    const stoneType = $stoneType.val();
    const growingType = $growingType.val();
    const stoneShape = $stoneShape.find('option:selected').text();

    loadPerStoneWeight(stoneType, growingType, stoneShape, lengthDiameter);
}

// debounce to reduce API calls
const debouncedMmHandler = debounce(onMmSizeChange, 300);

// wire events
$mmSize.on('input change', debouncedMmHandler);

// trigger once on load
if ($mmSize.val()) {
    onMmSizeChange();
}


// ======================================================
//  STONE COST PER CARAT LOADER
// ======================================================
function parseValOrText($el) {
    const v = ($el.val() || '').toString().trim();
    if (v) return v;
    return ($el.find('option:selected').text() || '').trim();
}


$ddlSizeRange.on('change', function () {
    return;
     const stoneType = $stoneType.val();
    const growingType = $growingType.val();
    const stoneShape = $stoneShape.find('option:selected').text() || '';
    const vendor = $stoneVendor.val();
    const sizeRange = $ddlSizeRange.val();  
    $.ajax({
        url: webRoot + '/api/sku/stonelengthdiameter',
        method: 'GET',
        data: {
            vendor: vendor,
            stoneType: stoneType,
            growingType: growingType,
            stoneShape: stoneShape,
            sizeRange: sizeRange    

        },
        beforeSend: function () {
            //$txtCost.prop('disabled', true);
            $('#txtPerStoneWt').val(''); // clear weight until we get new value 
        },
        success: function (res) {
            if (res && res.stonelengthdiameter !== undefined && res.stonelengthdiameter !== null) {
                const lengthdiameter = Number(res.stonelengthdiameter).toFixed(2);
                $mmSize.val(lengthdiameter);
                $mmSize.change(); // trigger change to load per-stone weight
                
            } else {
                $mmSize.val('0');
            }
        },
        error: function () {
            $mmSize.val('0');
        },
        complete: function () {
            //$txtCost.prop('disabled', false);
        }
    });
});

async function fetchAndSetCost() {
    const stoneQuality = parseValOrText($ddlQuality);
    const lengthDiameter = ($mmSize.val() || '').toString().trim();
    const stoneqty = $stoneQty.val() || 0;
    if (!stoneQuality || !lengthDiameter) {
        $txtCost.val('');
        return;
    }

    const stoneType = $stoneType.val();
    const growingType = $growingType.val();
    //const stoneShape = $stoneShape.find('option:selected').text() || '';
    const stoneShapeCpde = $stoneShape.val() || '';
    const vendor = $stoneVendor.val();
    $.ajax({
        url: webRoot + '/api/sku/stonecostpercarat',
        method: 'GET',
        data: {
            vendor: vendor,
            stoneType: stoneType,
            growingType: growingType,
            stoneShape: stoneShapeCpde,
            lengthDiameter: lengthDiameter,
            stoneQuality: stoneQuality
        },
        beforeSend: function () {
            //$txtCost.prop('disabled', true);
        },
        success: function (res) {
            if (res && res.stoneCostPerCarat !== undefined && res.stoneCostPerCarat !== null) {
                const cost = Number(res.stoneCostPerCarat).toFixed(2);
                $txtCost.val(cost);
                calculatetStoneTotalCost();
                //const qty = parseNumber($stoneQty.val())||0;
                //const per = parseFloat($txtPerStoneWt.val())||0;
                //const total = qty * per;

                //$txtTotalStoneWt.val(total.toFixed(3));
                //$txtTotalAdjStoneWt.val(total.toFixed(3));
                //const adjWt = parseFloat($txtTotalAdjStoneWt.val()) || parseFloat($txtTotalStoneWt.val()) || 0;
                //$txtStoneTotalCost.val((res.stoneCostPerCarat * adjWt).toFixed(2));
            } else {
                $txtCost.val('');
            }
        },
        error: function () {
            $txtCost.val('');
        },
        complete: function () {
            //$txtCost.prop('disabled', false);
        }
    });
}
function calculatetStoneTotalCost() {
    const qty = parseNumber($stoneQty.val()) || 0;
    const per = parseFloat($txtPerStoneWt.val()) || 0;
    const total = qty * per;
    const stonecostpercaret = $txtCost.val();
    $txtTotalStoneWt.val(total.toFixed(3));
    //$txtTotalAdjStoneWt.val(total.toFixed(3));
    const adjWt = parseFloat($txtTotalAdjStoneWt.val()) || parseFloat($txtTotalStoneWt.val()) || 0;
    $txtStoneTotalCost.val((parseFloat(stonecostpercaret) * adjWt).toFixed(2));
}
const costHandler = debounce(fetchAndSetCost, 250);

// wire dependent controls
$ddlQuality.on('change', costHandler);
$mmSize.on('input change', costHandler);
$ddlStoneType.on('change', costHandler);
$ddlGrowingType.on('change', costHandler);
$ddlStoneShape.on('change', costHandler);
$ddlSizeRange.on('change', costHandler);
//$stoneQty.on('input change', costHandler);
$stoneQty.on('input change', calculatetStoneTotalCost);

$ddlQuality.on('change', costHandler);
$txtTotalAdjStoneWt.on('input change', calculatetStoneTotalCost);


// initial load
if ($ddlQuality.val() || $mmSize.val()) {
    costHandler();
}


// ======================================================
//   SETTING VENDOR → SETTING TYPE LOADER
// ======================================================
$('#ddlSettingVendor').on('change', function () {
    const vendor = $(this).val();

    if (!vendor) {
        $('#ddlSettingType').empty().append('<option value=""></option>');
        $('#ddlSettingType').empty().append('<option value=""></option>');
        return;
    }

    $.get(webRoot + '/api/sku/settingtype?settingVendor=' + encodeURIComponent(vendor), function (data) {
        const settingType = $('#ddlSettingType');
        settingType.empty().append('<option value=""></option>');

        $.each(data, function (i, item) {
            settingType.append($('<option>').val(item.Key).text(item.Value));
        });
    });

    getTaxDetails('setting', vendor);
});


// ======================================================
//   SETTING COST PER STONE LOADER
// ======================================================
$('#ddlSettingType').on('change', function () {
    const vendor = $('#ddlSettingVendor').val();
    //const type = $('#ddlSettingType').val();
    const type = $('#ddlSettingType option:selected').text();
    const perStoneWt = $('#txtPerStoneWt').val();
    const stoneShape = $('#ddlStoneShape option:selected').text();
    const stoneShapeCode = $('#ddlStoneShape').val();
    const category = $('#ddlCategory option:selected').text().trim();
    const subCategory = $('#ddlSubCategory option:selected').text().trim();
    const errormsg = "Cost for given Vendor/Type not found.";
    var metal = 'Gold';
    if (metalLines.length > 0) {
        metal = metalLines[0].metalText;
    }
    $('#txtCostPerStone').val('');
    $('#txtTotalCost').val('');


    if (!vendor || !type || !perStoneWt) {
        $('#txtCostPerStone').val('');
        $('#txtTotalCost').val('');

        return;
    }

    $.get(webRoot + '/api/sku/settingcostperstone', {
        vendor: vendor,
        settingType: type,
        perStoneWt: perStoneWt,
        shapeCode: stoneShapeCode,
        shape: stoneShape,
        category: category,
        subCategory: subCategory,
        metal: metal    
    }, function (data) {
        if (data && data.costPerStone !== undefined) {
            $('#txtCostPerStone').val(Number(data.costPerStone).toFixed(2));
            updateTotalCost();
        } else {
            $('#txtCostPerStone').val('');
            $('#txtTotalCost').val('');
        }
    }).always(function () {
        if ($('#txtCostPerStone').val() === '') {
            setFieldError($("#ddlSettingType"), errormsg);
            $('#txtTotalCost').val('');
        }
        else {
            clearFieldError($("#ddlSettingType"));
        }
    });
});


// ======================================================
//   TOTAL COST CALCULATOR
// ======================================================
function updateTotalCost() {
    const cost = parseFloat($costPerStone.val()) || 0;
    const qty = parseFloat($stoneQty.val()) || 0;

    const total = cost * qty;
    $totalCost.val(total.toFixed(2));
}



/************************************************************
 * STONE TYPE → STONE SHAPE
 ************************************************************/
$('#ddlStoneType').on('change', function () {
    const stoneType = $(this).val();
    if (!stoneType) return;

    $.get(webRoot + '/api/sku/stoneshape/' + stoneType, function (data) {
        const stoneShapeDropdown = $('#ddlStoneShape');
        stoneShapeDropdown.empty().append('<option value=""></option>');

        $.each(data, function (i, item) {
            stoneShapeDropdown.append($('<option>').val(item.Key).text(item.Value));
        });
    });
});

/************************************************************
 * STONE SHAPE → STONE QUALITY
 ************************************************************/
$('#ddlStoneShape').on('change', function () {
    const stoneShape = $(this).val();
    const stoneType = $('#ddlStoneType').val();
    const growing = $('#ddlGrowing').val();
    const vendor = $('#ddlStoneVendor').val();
    $.get(webRoot + 'api/sku/stonequality/' +
        stoneType + '/' + growing + '/' + stoneShape,
        function (data) {
            const stoneQuality = $('#ddlStoneQuality');
            stoneQuality.empty().append('<option value=""></option>');

            $.each(data, function (i, item) {
                stoneQuality.append($('<option>').val(item.Key).text(item.Value));
            });
        });

    $.get(webRoot + 'api/sku/stonesizerange/' +
        stoneType + '/' + growing + '/' + stoneShape + '/' + vendor,
        function (data) {
            const stonesizerange = $('#ddlSizeRange');
            stonesizerange.empty().append('<option value=""></option>');

            $.each(data, function (i, item) {
                stonesizerange.append($('<option>').val(item.Key).text(item.Value));
            });
        });
});

/************************************************************
 * SETTING LOCATION LOGIC
 * - Center → enable Per Stone Weight
 * - Semi   → disable Per Stone Weight
 ************************************************************/
function updatePerStone() {
    const val = $('#ddlSettingLocation').val()?.toString().trim().toLowerCase();
    const $txt = $('#txtPerStoneWt');

    if (val === 'center') {
        $txt.prop('disabled', false).prop('readonly', false);
    } else if (val === 'semi') {
        $txt.prop('disabled', true).prop('readonly', true).val('');
    } else {
        $txt.prop('disabled', true).prop('readonly', true);
    }
}

$('#ddlSettingLocation').on('change', updatePerStone);
updatePerStone();

/************************************************************
 * TOTAL STONE WEIGHT CALCULATION
 ************************************************************/
//const $qty = $('#txtStoneQty');
//const $per = $('#txtPerStoneWt');


//function recalcTotal() {
//    const qty = parseNumber($qty.val());
//    const per = parseNumber($per.val());
//    const total = qty * per;

//    $txtTotalStoneWt.val(total.toFixed(3));
//    $txtTotalAdjStoneWt.val(total.toFixed(3));

//}

//$qty.on('input change', recalcTotal);
//$per.on('input change', recalcTotal);
//recalcTotal();

/************************************************************
 * PER-STONE WEIGHT API
 ************************************************************/
function loadPerStoneWeight(stoneType, growingType, stoneShape, lengthDiameter) {
    var errormsg = "Per Stone Wt not found for given size.";
    // If lengthDiameter is not provided or shorter than 4 characters, reset dependent fields and skip API call
    var ld = lengthDiameter ? lengthDiameter.toString().trim() : '';
    if (!ld || ld.length < 4) {
        $('#txtStoneWidth1').val('');
        $('#txtStoneWidth2').val('');
        $('#txtPerStoneWt').val('');
        $('#txtStoneTotalCost').val('');
        $('#ddlSizeRange').val('');
        return;
    }
    $.ajax({
        url: webRoot + '/api/sku/perstoneweight',
        method: 'GET',
        data: {
            stoneType: stoneType,
            growingType: growingType,
            stoneShape: stoneShape,
            lengthDiameter: lengthDiameter
        },
        success: function (res) {
            if (res && res.perStoneWeight !== undefined && res.perStoneWeight !== null) {
                $('#txtPerStoneWt').val(Number(res.perStoneWeight).toFixed(3));
            } else {
                $('#txtPerStoneWt').val('');
            }

            // Also update Width1, Width2 and SizeRange when API provides them (do not trigger change events)
            if (res && res.width1 !== undefined && res.width1 !== null) {
                $('#txtStoneWidth1').val(res.width1);
            } else {
                $('#txtStoneWidth1').val('');
            }

            if (res && res.width2 !== undefined && res.width2 !== null) {
                $('#txtStoneWidth2').val(res.width2);
            } else {
                $('#txtStoneWidth2').val('');
            }

            if (res && res.sizeRange !== undefined && res.sizeRange !== null) {
                // set dropdown value without triggering change
                $('#ddlSizeRange').val(res.sizeRange);
            } else {
                $('#ddlSizeRange').val('');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $('#txtPerStoneWt').val('');
            //errormsg = jqXHR.responseJSON.Message;
        },
        complete: function () {
            if ($('#txtPerStoneWt').val() === '') {
                setFieldError($("#txtStoneMMSize"), errormsg);
            }
            else {
                clearFieldError($("#txtStoneMMSize"));
            }
        }

    });
}

/************************************************************
 * STONE MODULE (TABLE RENDERING)
 ************************************************************/


let stoneEditIndex = -1; // -1 = add mode, >=0 = edit mode

function getStoneModel() {

    var totalCost = parseFloat($('#txtStoneTotalCost').val());
    var settingtotalCost = parseFloat($('#txtTotalCost').val());
    var settinglocation = $('#ddlSettingLocation').val();
        diamondDutyVal = totalCost * diamondDutyper;
        diamondPenaltyVal = totalCost * diamondPenaltyPer;
    diamondTariffVal = totalCost * diamondTariffPer;

    settingDutyVal = settingtotalCost * settingDutyper;
    settingPenaltyVal = settingtotalCost * settingPenaltyPer;
    settingTariffVal = settingtotalCost * settingTariffPer;

    //if (settinglocation === 'Semi') {
    //    diamondDutyVal = totalCost * diamondDutyper;
    //    diamondPenaltyVal = totalCost * diamondPenaltyPer;
    //    diamondTariffVal = totalCost * diamondTariffPer;
    //}
    //else if (settinglocation === 'Center') {
    //    diamondCenterDutyVal = totalCost * diamondDutyper;
    //    diamondCenterPenaltyVal = totalCost * diamondPenaltyPer;
    //    diamondCenterTariffVal = totalCost * diamondTariffPer;
    //}
    return {
        StoneVendorCode: $('#ddlStoneVendor').val(),
        StoneVendor: $('#ddlStoneVendor option:selected').text(),
        StoneType: $('#ddlStoneType').val(),
        Growing: $('#ddlGrowing').val(),
        SettingLocation: $('#ddlSettingLocation').val(),
        Lab: $('#ddlLab').val(),

        Shape: $('#ddlStoneShape').val(),
        ShapeText: $('#ddlStoneShape option:selected').text(),
        SizeRange: $('#ddlSizeRange').val(),

        MMSize: $('#txtStoneMMSize').val(),
        Width1: $('#txtStoneWidth1').val(),
        Width2: $('#txtStoneWidth2').val(),

        PerStoneWt: $('#txtPerStoneWt').val(),
        Qty: $('#txtStoneQty').val(),
        TotalStoneWt: $('#txtTotalStoneWt').val(),
        TotalAdjStoneWt: $('#txtTotalAdjStoneWt').val(),

        StoneQuality: $('#ddlStoneQuality').val(),
        StoneQualityText: $('#ddlStoneQuality option:selected').text(),
        StoneCostPerCarat: $('#txtStoneCostPerCarat').val(),
        StoneTotalCost: $('#txtStoneTotalCost').val(),

        SettingVendorCode: $('#ddlSettingVendor').val(),
        SettingVendor: $('#ddlSettingVendor option:selected').text(),
        SettingType: $('#ddlSettingType option:selected').text(),
        SettingTypeCode: $('#ddlSettingType').val(),
        CostPerStone: $('#txtCostPerStone').val(),
        TotalCost: $('#txtTotalCost').val(),

        SemiMinWt: $('#txtSemiMinWt').val(),
        SemiAdjWt: $('#txtSemiAdjWt').val(),
        CenterMinWt: $('#txtCenterMinWt').val(),
        CenterAdjWt: $('#txtCenterAdjWt').val(),
        TotalMinWt: $('#txtTotalMinWt').val(),
        TotalAdjWt: $('#txtTotalAdjWt').val(),
        StoneDutyVal: diamondDutyVal,
        StonePenaltyVal: diamondPenaltyVal,
        StoneTariffVal: diamondTariffVal,
        SettingDutyVal: settingDutyVal,
        SettingPenaltyVal: settingPenaltyVal,
        SettingTariffVal: settingTariffVal,
    };
}
//Added By Mahesh   Start
$('#txtSemiAdjWt').on('change', function () {
    totalSemiAdjWt = parseFloat($('#txtSemiAdjWt').val()) || 0
});

$('#txtCenterAdjWt').on('change', function () {
    totalSemiAdjWt = parseFloat($('#txtCenterAdjWt').val()) || 0
});
//Added By Mahesh   End


$('#btnStoneAddUpdate').on('click', function () {
    if (!validateButtonAddUpdate("btnStoneAddUpdate")) {
        return 0;
    }
    const model = getStoneModel();
    // Total Center and Simi cost accumulators
    if (model.SettingLocation === 'Center') {
        //totalCenterStoneCost += parseFloat(model.StoneTotalCost) || 0;
        //totalCenterWt += parseFloat(model.TotalStoneWt) || 0;
        //totalCenterSettingCost += parseFloat(model.TotalCost) || 0;
        //totalCenterAdjWt += parseFloat(model.TotalAdjStoneWt) || 0;//added By Mahesh
        //$('#txtCenterAdjWt').prop('disabled', false);

    }
    else {
        //totalSemiStoneCost += parseFloat(model.StoneTotalCost) || 0;
        //totalSemiWt += parseFloat(model.TotalStoneWt) || 0;
        //totalSemiSettingCost += parseFloat(model.TotalCost) || 0;
        //totalSemiAdjWt += parseFloat(model.TotalAdjStoneWt) || 0;  //added By Mahesh
        //$('#txtSemiAdjWt').prop('disabled', false);
    }
    //$('#txtSemiMinWt').val(parseFloat(totalSemiWt).toFixed(3));
    //$('#txtCenterMinWt').val(parseFloat(totalCenterWt).toFixed(3));
   
   
    if (stoneEditIndex === -1) {
        // ADD
        stoneList.push(model);
    } else {
        // UPDATE
        stoneList[stoneEditIndex] = model;
        stoneEditIndex = -1;
        $('#btnStoneAddUpdate').text('Add Stone');
    }

    renderStoneTable();
     $('#txtSemiAdjWt').val(parseFloat(totalSemiAdjWt).toFixed(3));//added By Mahesh
    $('#txtCenterAdjWt').val(parseFloat(totalCenterAdjWt).toFixed(3));//added By Mahesh
    calculateTotals();
    clearStoneControls();
    // $("#ddlStoneVendor").focus();
});

function renderStoneTable() {
    let html = '';
    totalStoneQty = 0;
    totalTotalStoneWt = 0;
    totalTotalAdjStoneWt = 0;
    totalCosttotal = 0;
    $('#txtCenterAdjWt').prop('disabled', true);
    $('#txtSemiAdjWt').prop('disabled', true);
   

    stoneList.sort(function (a, b) {

        // 1. SettingLocation (string)
        if ((a.SettingLocation || '') !== (b.SettingLocation || '')) {
            return (a.SettingLocation || '').localeCompare(b.SettingLocation || '');
        }

        // 2. ShapeText (string)
        if ((a.ShapeText || '') !== (b.ShapeText || '')) {
            return (a.ShapeText || '').localeCompare(b.ShapeText || '');
        }

        // 3. PerStoneWt (numeric)
        return (parseFloat(a.PerStoneWt) || 0) - (parseFloat(b.PerStoneWt) || 0);
    });
    diamondtotalCentertax = 0.00;
    diamondtotalSemitax = 0.00;
    totalCenterSettingCost = 0.00;
    totalSemiSettingCost = 0.00;
    totalCenterAdjWt = 0.00;
    totalSemiAdjWt = 0.00;
    totalCenterStoneCost = 0.00;
    totalSemiStoneCost = 0.00;
    totalCenterWt = 0.00;
    totalSemiWt = 0.00;
    stoneList.forEach((s, i) => {
        totalStoneQty = parseInt(totalStoneQty) + parseInt(s.Qty);
        if (s.SettingLocation === 'Center') {
            $('#txtCenterAdjWt').prop('disabled', true  );
            totalCenterStoneCost += parseFloat(s.StoneTotalCost) || 0;
            totalCenterWt += parseFloat(s.TotalStoneWt) || 0;
            totalCenterSettingCost += parseFloat(s.TotalCost) || 0;
            totalCenterAdjWt += parseFloat(s.TotalAdjStoneWt) || 0;//added By Mahesh
            diamondtotalCentertax += (parseFloat(s.StoneDutyVal) + parseFloat(s.StonePenaltyVal) + parseFloat(s.StoneTariffVal));
            //totalCenterSettingCost += s.TotalCost;
        }else{
            $('#txtSemiAdjWt').prop('disabled', true);
            totalSemiStoneCost += parseFloat(s.StoneTotalCost) || 0;
            totalSemiWt += parseFloat(s.TotalStoneWt) || 0;
            totalSemiSettingCost += parseFloat(s.TotalCost) || 0;
            totalSemiAdjWt += parseFloat(s.TotalAdjStoneWt) || 0;  //added By Mahesh
            diamondtotalSemitax += (parseFloat(s.StoneDutyVal) + parseFloat(s.StonePenaltyVal) + parseFloat(s.StoneTariffVal));
            //totalSemiSettingCost += s.TotalCost;
        }
        html += `
            <tr>
                <td>${s.StoneVendor}</td>
                <td>${s.SettingLocation}</td>
                <td>${s.ShapeText}</td>
                <td class="text-center">${s.MMSize}</td>
                <td class="text-center">${parseFloat(s.PerStoneWt).toFixed(3) }</td>
                <td class="text-center">${s.Qty}</td>
                <td class="text-center d-none">${s.TotalStoneWt}</td>
                <td class="text-center">${parseFloat(s.TotalAdjStoneWt).toFixed(3) }</td >
                <td>${s.SettingVendor}</td>
                <td class="text-center">$${parseFloat((parseFloat(s.TotalCost) + parseFloat(s.StoneTotalCost))).toFixed(2) }</td>
                <td class="text-center">
                    <button class="btn btn-sm btn-info m-1" onclick="setStoneData(${i})">View</button>
                    <button class="btn btn-sm btn-danger  m-1" onclick="deleteStone(${i})">Delete</button>
                </td>
            </tr>
        `;
       // totalStoneQty = parseInt(totalStoneQty) + parseInt(s.Qty);
        totalTotalStoneWt = parseFloat(totalTotalStoneWt) + parseFloat(s.TotalStoneWt);
        totalTotalAdjStoneWt = parseFloat(totalTotalAdjStoneWt) + parseFloat(s.TotalAdjStoneWt);
        totalCosttotal = parseFloat(totalCosttotal) + parseFloat(s.TotalCost) + parseFloat(s.StoneTotalCost);
    });
    htmlFoot = `
            <tr>
                <td></b>Total</b></td>
                <td></td>
                <td></td>
                <td></td>
                                <td></td>
                <td class="text-center">${totalStoneQty}</td>
                <td class="text-center d-none">${totalTotalStoneWt.toFixed(3)}</td>
                <td class="text-center">${totalTotalAdjStoneWt.toFixed(3)}</td>
                <td></td>
                <td class="text-center">$${totalCosttotal.toFixed(2)}</td>
                <td class="text-nowrap">
                    
                </td>
            </tr>
        `;
    if (stoneList.length > 0) {
        $('#tblStone tfoot').show();
    } else {
        $('#tblStone tfoot').hide();
    }
    $('#tblStone tbody').html(html);
    $('#tblStone tfoot').html(htmlFoot);
}

function editStone(index) {
    const s = stoneList[index];
    stoneEditIndex = index;

    //$('#ddlStoneVendor').val(s.StoneVendor);
    //$('#ddlStoneType').val(s.StoneType);
    setValue('#ddlStoneVendor', s.StoneVendor);
    setValue('#ddlStoneType', s.StoneType);
    // ✅ Normalize value
    if (s.Growing === "Lab - HPHT  CVD") {
        growingtype = "Lab - HPHT / CVD";
        setSelectedText('#ddlGrowing', growingtype);
    }
    else {
        //$('#ddlGrowing').val(s.Growing);
        setSelectedText('#ddlGrowing', s.Growing);
    }



    //$('#ddlSettingLocation').val(s.SettingLocation);
    setValue('#ddlSettingLocation', s.SettingLocation);
    //$('#ddlLab').val(s.Lab);
    setValue('#ddlLab', s.Lab);
    //setSelectedText('#ddlStoneShape', s.Shape);
    setValue('#ddlStoneShape', s.Shape);
    setValue('#ddlSizeRange', s.SizeRange);
    //$('#ddlStoneShape').val(s.Shape);
    $('#txtStoneMMSize').val(s.MMSize);
    $('#txtStoneWidth1').val(s.Width1);
    $('#txtStoneWidth2').val(s.Width2);

    $('#txtPerStoneWt').val(s.PerStoneWt);
    $('#txtStoneQty').val(s.Qty);
    $('#txtTotalStoneWt').val(s.TotalStoneWt);
    $('#txtTotalAdjStoneWt').val(s.TotalAdjStoneWt);

    //$('#ddlStoneQuality').val(s.StoneQuality);
    setSelectedText('#ddlStoneQuality', s.StoneQuality);
    $('#txtStoneCostPerCarat').val(s.StoneCostPerCarat);
    $('#txtStoneTotalCost').val(s.StoneTotalCost);

    $('#ddlSettingVendor').val(s.SettingVendor);
    $('#ddlSettingType').val(s.SettingType);
    $('#txtCostPerStone').val(s.CostPerStone);
    $('#txtTotalCost').val(s.TotalCost);

    $('#txtSemiMinWt').val(s.SemiMinWt);
    $('#txtSemiAdjWt').val(s.SemiAdjWt);
    $('#txtCenterMinWt').val(s.CenterMinWt);
    $('#txtCenterAdjWt').val(s.CenterAdjWt);
    $('#txtTotalMinWt').val(s.TotalMinWt);
    $('#txtTotalAdjWt').val(s.TotalAdjWt);

    $('#btnStoneAddUpdate').text('Update Stone');
}

function deleteStone(index) {

    if (stoneList[index].SettingLocation === 'Center') {
        totalCenterStoneCost -= parseFloat(stoneList[index].StoneTotalCost) || 0;
        totalCenterWt -= parseFloat(stoneList[index].TotalStoneWt) || 0;
        totalCenterSettingCost -= parseFloat(stoneList[index].TotalCost) || 0;
        totalCenterAdjWt -= parseFloat(stoneList[index].TotalAdjStoneWt) || 0;//added By Mahesh


    }
    else {
        totalSemiStoneCost -= parseFloat(stoneList[index].StoneTotalCost) || 0;
        totalSemiWt -= parseFloat(stoneList[index].TotalStoneWt) || 0;
        totalSemiSettingCost -= parseFloat(stoneList[index].TotalCost) || 0;
        totalSemiAdjWt -= parseFloat(stoneList[index].TotalAdjStoneWt) || 0;  //added By Mahesh

    }
    totalStoneQty = parseInt(totalStoneQty) - parseInt(stoneList[index].Qty);
    //$('#txtSemiMinWt').val(Number(totalSemiWt).toFixed(3));
    //$('#txtCenterMinWt').val(Number(totalCenterWt).toFixed(3));

    $('#txtSemiAdjWt').val(Number(totalSemiAdjWt).toFixed(3));//added By Mahesh
    $('#txtCenterAdjWt').val(Number(totalCenterAdjWt).toFixed(3));//added By Mahesh

   
    stoneList.splice(index, 1);
    calculateTotals();
    renderStoneTable();
}

function setStoneData(index) {
    settingData = true;
    var data = stoneList[index];
    stoneEditIndex = index;
    $('#btnStoneAddUpdate').text("Update Stone");
    $('#ddlStoneVendor').val(data.StoneVendorCode).addClass('disabled').attr('disabled', true);
    $('#ddlStoneType').val(data.StoneType).trigger('change').addClass('disabled').attr('disabled', true);

    $('#ddlGrowing').val(data.Growing).addClass('disabled').attr('disabled', true);
    $('#ddlSettingLocation').val(data.SettingLocation).addClass('disabled').attr('disabled', true);
    $('#ddlLab').val(data.Lab).addClass('disabled').attr('disabled', true);
    setTimeout(function () {
        $('#ddlStoneShape').val(data.Shape).trigger('change').addClass('disabled').attr('disabled', true);
        setTimeout(function () {
            $('#ddlStoneQuality').val(data.StoneQuality).addClass('disabled').attr('disabled', true);
            $('#txtStoneCostPerCarat').val(data.StoneCostPerCarat).addClass('disabled').attr('disabled', true);
            $('#ddlSizeRange').val(data.SizeRange);//.addClass('disabled').attr('disabled', true); 

        }, 800);
    }, 800);
    //$('#ddlStoneShape').val(data.Shape);
    // ShapeText usually not set manually (comes from dropdown)

    $('#txtStoneMMSize').val(data.MMSize);//.addClass('disabled').attr('disabled', true);
    $('#txtStoneWidth1').val(data.Width1).addClass('disabled').attr('disabled', true);
    $('#txtStoneWidth2').val(data.Width2).addClass('disabled').attr('disabled', true);

    $('#txtPerStoneWt').val(data.PerStoneWt).addClass('disabled').attr('disabled', true);
    $('#txtStoneQty').val(data.Qty);//.addClass('disabled').attr('disabled', true);
    $('#txtTotalStoneWt').val(data.TotalStoneWt);
    $('#txtTotalAdjStoneWt').val(data.TotalAdjStoneWt);

    // $('#ddlStoneQuality').val(data.StoneQuality);

    $('#txtStoneTotalCost').val(data.StoneTotalCost).addClass('disabled').attr('disabled', true);

    $('#ddlSettingVendor').val(data.SettingVendorCode).addClass('disabled').attr('disabled', true).trigger('change');
    setTimeout(function () {
        $('#ddlSettingType').val(data.SettingTypeCode).addClass('disabled').attr('disabled', true);
    }, 800);
    //$('#ddlSettingType').val(data.SettingType);
    $('#txtCostPerStone').val(data.CostPerStone).addClass('disabled').attr('disabled', true);
    $('#txtTotalCost').val(data.TotalCost).addClass('disabled').attr('disabled', true);
    //$('#txtSemiMinWt').val(data.SemiMinWt);
    //$('#txtSemiAdjWt').val(data.SemiAdjWt);
    //$('#txtCenterMinWt').val(data.CenterMinWt);
    //$('#txtCenterAdjWt').val(data.CenterAdjWt);
    //$('#txtTotalMinWt').val(data.TotalMinWt);
    //$('#txtTotalAdjWt').val(data.TotalAdjWt);
    $('html, body').animate({ scrollTop: 0 }, 800);
    /*$('#btnStoneAddUpdate').addClass('disabled').attr('disabled', true)*/
    
    settingData = false;
}

function clearStoneControls() {
    settingData = true;
    $('#ddlStoneVendor').val('').removeClass('disabled').attr('disabled', false);
    $('#ddlStoneType').val('').removeClass('disabled').attr('disabled', false);
    $('#ddlGrowing').val('').removeClass('disabled').attr('disabled', false);
    $('#ddlSettingLocation').val('').removeClass('disabled').attr('disabled', false);
    $('#ddlLab').val('').removeClass('disabled').attr('disabled', false);
    $('#ddlStoneShape').val('').removeClass('disabled').attr('disabled', false);
    //$('#txtSizeRange').val('');
    $('#ddlSizeRange').val('').removeClass('disabled').attr('disabled', false);
    $('#txtStoneMMSize').val('').removeClass('disabled').attr('disabled', false);
    $('#txtStoneWidth1').val('').removeClass('disabled').attr('disabled', false);
    $('#txtStoneWidth2').val('').removeClass('disabled').attr('disabled', false);
    $('#txtPerStoneWt').val('');
    $('#txtStoneQty').val('').removeClass('disabled').attr('disabled', false);
    $('#txtTotalStoneWt').val('');
    $('#txtTotalAdjStoneWt').val('').removeClass('disabled').attr('disabled', false);

    $('#ddlStoneQuality').val('').removeClass('disabled').attr('disabled', false);
    $('#txtStoneCostPerCarat').val('');
    $('#txtStoneTotalCost').val('');

    $('#ddlSettingVendor').val('').removeClass('disabled').attr('disabled', false);
    $('#ddlSettingType').val('').removeClass('disabled').attr('disabled', false);
    $('#txtCostPerStone').val('');
    $('#txtTotalCost').val('');
    //$('#btnStoneAddUpdate').removeClass('disabled').attr('disabled', false);
    $('#btnStoneAddUpdate').text("Add Stone");
    settingData=false

}


/************************************************************
 *  Script Section 7 — LABOR MODULE
 *  ---------------------------------------------------------
 *  Contains:
 *   - fillLaborFOBValues()
 *   - calculateTotalLabor()
 *   - getLaborObject()
 *   - setLaborObject()
 *   - clearLaborControls() (optional)
 ************************************************************/

/**
 * Read all labor-related controls and return a structured object.
 */

// Helper: round up to nearest 0.25
function roundUpToQuarter(value) {
    return Math.ceil(value * 4) / 4;
}

function getMarginDetails() {
    const vendor = $('#ddlCompany').val();
    const category = $('#ddlCategory').val();
    const subCategory = $('#ddlSubCategory').val();
    var metal = 'Gold';
    if (metalLines.length > 0) {
        metal = metalLines[0].metalText;
    }
    const url = webRoot + '/api/sku/margindetails/' + encodeURIComponent(vendor) + '/' + encodeURIComponent(category) + '/' + encodeURIComponent(subCategory) + '/' + encodeURIComponent(metal);

    $.getJSON(url, function (data) {
        if (!data) return;

        semiPrice1Per = (data.PMargin1 / 100) || 0;
        semiPrice2Per = (data.PMargin2 / 100) || 0;
        semiPrice3Per = (data.PMargin3 / 100) || 0;
        semiPrice4Per = (data.PMargin4 / 100) || 0;
        centerPrice1Per = (data.PMargin1 / 100) || 0;
        centerPrice2Per = (data.PMargin2 / 100) || 0;
        centerPrice3Per = (data.PMargin3 / 100) || 0;
        centerPrice4Per = (data.PMargin4 / 100) || 0;
        fillLaborFOBValues();
    });
}
function fillLaborFOBValues() {

     //getMarginDetails();
    //if (laborTariffPer > 0 || laborDutyper > 0 || laborPenaltyPer> 0) {
        settingtotalCentertax = 0.00;
        settingtotalSemitax = 0.00;

        stoneList.forEach((s, i) => {
            
            //s.SettingDutyVal = (parseFloat(s.TotalCost) * settingDutyper);
            //s.SettingPenaltyVal = (parseFloat(s.TotalCost) * settingPenaltyPer);
            //s.SettingTariffVal = (parseFloat(s.TotalCost) * settingTariffPer);
            if (s.SettingLocation === "Semi") {
                settingtotalSemitax += s.SettingDutyVal + s.SettingPenaltyVal + s.SettingTariffVal;
            } else {
                settingtotalCentertax += s.SettingDutyVal + s.SettingPenaltyVal + s.SettingTariffVal;

            }
        });
        //settingtotalCentertax = (totalCenterSettingCost * laborTariffPer)
    //}

    var landedcost = 0.00;
    var landedcostCenter = 0.00;
    totalFindingCost = 0;
    findingLines.forEach((row, idx) => {
        totalFindingCost += parseFloat(row.totalCost)||0;
    });
    totalMetalCost = 0;
    metalLines.forEach((row, idx) => {
        totalMetalCost += parseFloat(row.metalCost)||0;
    });
    //totalCenterSettingCost = 0;
    //totalCenterStoneCost = 0;
    //totalSemiStoneCost = 0;
    //totalSemiSettingCost = 0;
    //stoneList.forEach((row, idx) => {
       
   
    //    if (row.SettingLocation === 'Center') {
    //        totalCenterStoneCost += parseFloat(row.StoneTotalCost) || 0;
        
    //        totalCenterSettingCost += parseFloat(row.TotalCost) || 0;
        

    //}
    //else {
    //        totalSemiStoneCost += parseFloat(row.StoneTotalCost) || 0;
    //        totalSemiSettingCost += parseFloat(row.TotalCost) || 0;

    //    }
    //});

    // Calculate Semi FOB (without center stone)
    if ($('#ddlProcessType').val() === 'Flat Labour per piece' || $('#ddlProcessType').val() === 'Flat Labour per gm') {
        totalLaborCost = $('#txtTotalLabor').val() || 0;  
        totalLaborCostWTDuty = 0;
    }
    const semiFOBNRound = parseFloat(totalMetalCost)
        + parseFloat(totalFindingCost)
        + parseFloat(totalSemiStoneCost)
        + parseFloat(totalSemiSettingCost)
        + (parseFloat(totalLaborCost) - parseFloat(totalLaborCostWTDuty));

    const semiFOB = roundUpToQuarter(semiFOBNRound);



    // Calculate Complete FOB (with center stone)
    const completeFOBNRound = parseFloat(semiFOB) + parseFloat(totalCenterStoneCost) + parseFloat(totalCenterSettingCost);
    const completeFOB = roundUpToQuarter(completeFOBNRound);


    // Duty rates
    //var LaborLocation = $('#ddlLaborLocation').val();
    var LaborLocation = 'India'
    if ($('#ddlLaborLocation').val().length > 2) {
        var vendorlist = $('#ddlLaborLocation').val().split('|');
         LaborLocation = encodeURIComponent(vendorlist[1]);
    }
    
    ////if (LaborLocation === 'USA') {
    ////    dutyPer = 0.0;
    ////} else {
    //    const filtered = msdDutyDetails.filter(d => d.VendorLocation === LaborLocation);        // Sum Duty values
    //    const totalDuty = filtered.reduce((sum, d) => sum + d.Duty + d.Penalty + d.Tariff, 0);
    //    dutyPer = totalDuty / 100.0;
    ////}



    //if (dutyPer >= 0.0) {
    //    semiDuty = parseFloat(semiFOB) * dutyPer;
    //    centerDuty = semiDuty + (parseFloat(totalCenterStoneCost) * dutyPer);
    //    landedcost = parseFloat(semiFOB) + parseFloat(semiDuty);
    //    landedcostCenter = parseFloat(completeFOB) + parseFloat(centerDuty);
    //}
    //else {
    //    landedcost = parseFloat(semiFOB);
    //    landedcostCenter = parseFloat(completeFOB);
    //}


    var taxableLabor = (parseFloat(totalLaborCost) - parseFloat(totalLaborCostWTDuty))
    var laborSemiTax = 0;
    var laborCenterTax = 0;
    LaborDutyVal = taxableLabor * laborDutyper;
    LaborTariffVal = taxableLabor * laborTariffPer;
    LaborPenaltyVal = taxableLabor * laborPenaltyPer;

    laborSemiTax = LaborDutyVal + LaborPenaltyVal + LaborTariffVal + settingtotalSemitax;
    laborCenterTax = laborSemiTax + settingtotalCentertax;

    semiDuty = parseFloat(vendortotaltax) + parseFloat(findingtotaltax) + parseFloat(diamondtotalSemitax) + parseFloat(laborSemiTax);// + parseFloat(laborSemiTax);
    centerDuty = semiDuty + parseFloat(diamondtotalCentertax) + parseFloat(settingtotalCentertax);

    landedcost = parseFloat(semiFOB) + parseFloat(semiDuty);
    landedcostCenter = parseFloat(completeFOB) + parseFloat(centerDuty);

    landedcost = parseFloat(semiFOB) + parseFloat(semiDuty);
    landedcostCenter = parseFloat(completeFOB) + parseFloat(centerDuty);

    const semiPrice1 = (parseFloat(semiFOB) / (1 - semiPrice1Per)) + parseFloat(semiDuty) + parseFloat(totalLaborCostWTDuty);
    const semiPrice2 = (parseFloat(semiFOB) / (1 - semiPrice2Per)) + parseFloat(semiDuty) + parseFloat(totalLaborCostWTDuty);
    const semiPrice3 = (parseFloat(semiFOB) / (1 - semiPrice3Per)) + parseFloat(semiDuty) + parseFloat(totalLaborCostWTDuty);
    const semiPrice4 = (parseFloat(semiFOB) / (1 - semiPrice4Per)) + parseFloat(semiDuty) + parseFloat(totalLaborCostWTDuty);

    const centerPrice1 = (parseFloat(completeFOB) / (1 - centerPrice1Per)) + parseFloat(centerDuty) + parseFloat(totalLaborCostWTDuty);
    const centerPrice2 = (parseFloat(completeFOB) / (1 - centerPrice2Per)) + parseFloat(centerDuty) + parseFloat(totalLaborCostWTDuty);
    const centerPrice3 = (parseFloat(completeFOB) / (1 - centerPrice3Per)) + parseFloat(centerDuty) + parseFloat(totalLaborCostWTDuty);
    const centerPrice4 = (parseFloat(completeFOB) / (1 - centerPrice4Per)) + parseFloat(centerDuty) + parseFloat(totalLaborCostWTDuty);

    const semiMargin1 = ((semiPrice1 - (parseFloat(semiDuty) + parseFloat(semiFOB))) / semiPrice1) * 100;
    const semiMargin2 = ((semiPrice2 - (parseFloat(semiDuty) + parseFloat(semiFOB))) / semiPrice2) * 100;
    const semiMargin3 = ((semiPrice3 - (parseFloat(semiDuty) + parseFloat(semiFOB))) / semiPrice3) * 100;
    const semiMargin4 = ((semiPrice4 - (parseFloat(semiDuty) + parseFloat(semiFOB))) / semiPrice4) * 100;

    const centerMargin1 = ((centerPrice1 - (parseFloat(centerDuty) + parseFloat(completeFOB))) / centerPrice1) * 100;
    const centerMargin2 = ((centerPrice2 - (parseFloat(centerDuty) + parseFloat(completeFOB))) / centerPrice2) * 100;
    const centerMargin3 = ((centerPrice3 - (parseFloat(centerDuty) + parseFloat(completeFOB))) / centerPrice3) * 100;
    const centerMargin4 = ((centerPrice4 - (parseFloat(centerDuty) + parseFloat(completeFOB))) / centerPrice4) * 100;

    // Update SKU Module calculations
    skuModule.calculations.totalMetalCost = parseFloat(totalMetalCost).toFixed(2);
    skuModule.calculations.totalFindingCost = parseFloat(totalFindingCost).toFixed(2);
    skuModule.calculations.totalSemiStoneCost = parseFloat(totalSemiStoneCost).toFixed(2);
    skuModule.calculations.totalCenterStoneCost = parseFloat(totalCenterStoneCost).toFixed(2);
    skuModule.calculations.totalSemiSettingCost = parseFloat(totalSemiSettingCost).toFixed(2);
    skuModule.calculations.totalCenterSettingCost = parseFloat(totalCenterSettingCost).toFixed(2);
    skuModule.calculations.totalLaborCost = parseFloat(totalLaborCost).toFixed(2);
    skuModule.calculations.semiDuty = parseFloat(semiDuty).toFixed(0);



    skuModule.calculations.semiFOB = parseFloat(semiFOB).toFixed(2);
    skuModule.calculations.completeFOB = parseFloat(completeFOB).toFixed(2);

    skuModule.calculations.landedcost = parseFloat(landedcost).toFixed(2);
    skuModule.calculations.landedcostCenter = parseFloat(landedcostCenter).toFixed(2);

    // Prices: round up to whole numbers (no decimals)
    skuModule.calculations.semiPrice1 = (Math.ceil(semiPrice1)).toFixed(0);
    skuModule.calculations.semiPrice2 = (Math.ceil(semiPrice2)).toFixed(0);
    skuModule.calculations.semiPrice3 = (Math.ceil(semiPrice3)).toFixed(0);
    skuModule.calculations.semiPrice4 = (Math.ceil(semiPrice4)).toFixed(0);
    skuModule.calculations.centerPrice1 = (Math.ceil(centerPrice1)).toFixed(0);
    skuModule.calculations.centerPrice2 = (Math.ceil(centerPrice2)).toFixed(0);
    skuModule.calculations.centerPrice3 = (Math.ceil(centerPrice3)).toFixed(0);
    skuModule.calculations.centerPrice4 = (Math.ceil(centerPrice4)).toFixed(0);
    skuModule.calculations.semiMargin1 = semiMargin1.toFixed(0);
    skuModule.calculations.semiMargin2 = semiMargin2.toFixed(0);
    skuModule.calculations.semiMargin3 = semiMargin3.toFixed(0);
    skuModule.calculations.semiMargin4 = semiMargin4.toFixed(0);
    skuModule.calculations.centerMargin1 = centerMargin1.toFixed(0);
    skuModule.calculations.centerMargin2 = centerMargin2.toFixed(0);
    skuModule.calculations.centerMargin3 = centerMargin3.toFixed(0);
    skuModule.calculations.centerMargin4 = centerMargin4.toFixed(0);



    // Fill textboxes
    const txtSemiFOB = document.getElementById("txtSemiFOB");
    const txtCompleteFOB = document.getElementById("txtCompleteFOB");
    const txtSemiDuty = document.getElementById("txtSemiDuty");
    const txtCompleteDuty = document.getElementById("txtCompleteDuty");

    const txtPrice1 = document.getElementById("txtPrice1");
    const txtPrice2 = document.getElementById("txtPrice2");
    const txtPrice3 = document.getElementById("txtPrice3");
    const txtPrice4 = document.getElementById("txtPrice4");
    const txtMargin1 = document.getElementById("txtMargin1");
    const txtMargin2 = document.getElementById("txtMargin2");
    const txtMargin3 = document.getElementById("txtMargin3");
    const txtMargin4 = document.getElementById("txtMargin4");
    const txtCompletePrice1 = document.getElementById("txtCompletePrice1");
    const txtCompletePrice2 = document.getElementById("txtCompletePrice2");
    const txtCompletePrice3 = document.getElementById("txtCompletePrice3");
    const txtCompletePrice4 = document.getElementById("txtCompletePrice4");
    const txtCompleteMargin1 = document.getElementById("txtCompleteMargin1");
    const txtCompleteMargin2 = document.getElementById("txtCompleteMargin2");
    const txtCompleteMargin3 = document.getElementById("txtCompleteMargin3");
    const txtCompleteMargin4 = document.getElementById("txtCompleteMargin4");
    const txtLandedCost = document.getElementById("txtLandedCost");
    const txtLandedCostComplete = document.getElementById("txtLandedCostComplete");


    if (txtSemiFOB) txtSemiFOB.value = parseFloat(semiFOB).toFixed(2);
    if (txtCompleteFOB) txtCompleteFOB.value = parseFloat(completeFOB).toFixed(2);
    if (txtSemiDuty) txtSemiDuty.value = parseFloat(semiDuty).toFixed(2);
    if (txtCompleteDuty) txtCompleteDuty.value = parseFloat(centerDuty).toFixed(2);
    if (txtPrice1) txtPrice1.value = Math.ceil(parseFloat(semiPrice1 || 0));
    if (txtPrice2) txtPrice2.value = Math.ceil(parseFloat(semiPrice2 || 0));
    if (txtPrice3) txtPrice3.value = Math.ceil(parseFloat(semiPrice3 || 0));
    if (txtPrice4) txtPrice4.value = Math.ceil(parseFloat(semiPrice4 || 0));
    if (txtMargin1) txtMargin1.value = semiMargin1.toFixed(0);
    if (txtMargin2) txtMargin2.value = semiMargin2.toFixed(0);
    if (txtMargin3) txtMargin3.value = semiMargin3.toFixed(0);
    if (txtMargin4) txtMargin4.value = semiMargin4.toFixed(0);
    if (txtCompletePrice1) txtCompletePrice1.value = Math.ceil(parseFloat(centerPrice1 || 0));
    if (txtCompletePrice2) txtCompletePrice2.value = Math.ceil(parseFloat(centerPrice2 || 0));
    if (txtCompletePrice3) txtCompletePrice3.value = Math.ceil(parseFloat(centerPrice3 || 0));
    if (txtCompletePrice4) txtCompletePrice4.value = Math.ceil(parseFloat(centerPrice4 || 0));
    if (txtCompleteMargin1) txtCompleteMargin1.value = centerMargin1.toFixed(0);
    if (txtCompleteMargin2) txtCompleteMargin2.value = centerMargin2.toFixed(0);
    if (txtCompleteMargin3) txtCompleteMargin3.value = centerMargin3.toFixed(0);
    if (txtCompleteMargin4) txtCompleteMargin4.value = centerMargin4.toFixed(0);
    if (txtLandedCost) txtLandedCost.value = parseFloat(landedcost).toFixed(2);
    if (txtLandedCostComplete) txtLandedCostComplete.value = parseFloat(landedcostCenter).toFixed(2);

}

$('#ddlLaborLocation').on('change', function () {

    //fillLaborFOBValues();
});

function calculateTotalLabor() {
    // Helper to safely parse numbers
    function getValue(id) {
        const el = document.getElementById(id);
        if (!el) return 0;
        const val = parseFloat(el.value);
        return isNaN(val) ? 0 : val;
    }


    // Active textboxes
    const castPcs = getValue("txtCastPcs");
    const model = getValue("txtModel");
    const giftBox = getValue("txtGiftBox");

    // Disabled controls
    const cfp = getValue("txtCFP");
    const rhodium = getValue("txtRhodium");
    //const assembly = getValue("txtAssembly");
    //const solder = getValue("txtSolder");
    //const tag = getValue("txtTag");
    const diaHandle = getValue("txtDiaHandling");
    const finHandle = getValue("txtFinHandling");
    const stamping = getValue("txtStamping");
    const cam = getValue("txtCAM");
    const other1 = getValue("txtOtherCost1");
    const other2 = getValue("txtOtherCost2");
    const other3 = getValue("txtOtherCost3");

    const totalLaborEl = document.getElementById("txtTotalLabor");
    var totalLabor = 0;
    if ($('#ddlProcessType').val() == 'Flat Labour per gm' || $('#ddlProcessType').val() == 'Flat Labour per piece') {
        totalLabor = totalLaborEl.value;
        totalLaborCostWTDuty = parseFloat(totalLaborEl.value);
    }
    else {
        // Total calculation
        totalLabor = model + giftBox + cfp + rhodium + diaHandle + finHandle + stamping + cam + other1 + other2 + other3;
        totalLaborCostWTDuty = model + giftBox + cam; // + other1 + other2 + other3;
    }
    // Fill into txtTotalLabor

    if (totalLaborEl) {
        totalLaborEl.value = parseFloat(totalLabor).toFixed(2);
        totalLaborCost = parseFloat(totalLabor).toFixed(2); // Update global total labor cost
        fillLaborFOBValues();
    }
}



function getLabor() {
    var LaborLocation = '';
    var vendorCode = '';
    if ($('#ddlLaborLocation').val().length > 2) {
        var vendorlist = $('#ddlLaborLocation').val().split('|');
        vendorCode = vendorlist[0];
        LaborLocation = vendorlist[1];
    }
    return {
        LaborLocation: LaborLocation,
        VendorCode: vendorCode,
        VendorName: $('#ddlLaborLocation option:selected').text().trim(),
        ProcessType: $('#ddlProcessType').val(),
        CastingLabor: 0,//$('#txtCastingLabor').val(),
        CastPcs: 0,//$('#txtCastPcs').val(),
        CFP: $('#txtCFP').val(),
        Rhodium: $('#txtRhodium').val(),
        Assembly: $('#txtAssembly').val(),
        Solder: $('#txtSolder').val(),
        Tag: $('#txtTag').val(),
        DiaHandling: $('#txtDiaHandling').val(),
        FinHandling: $('#txtFinHandling').val(),
        Stamping: $('#txtStamping').val(),
        Model: $('#txtModel').val(),
        CAM: $('#txtCAM').val(),
        GiftBox: $('#txtGiftBox').val(),
        TotalLabor: $('#txtTotalLabor').val(),

        OtherHead1: $('#ddlOtherHead1').val(),
        OtherCost1: $('#txtOtherCost1').val(),
        OtherHead2: $('#ddlOtherHead2').val(),
        OtherCost2: $('#txtOtherCost2').val(),
        OtherHead3: $('#ddlOtherHead3').val(),
        OtherCost3: $('#txtOtherCost3').val(),

        SemiFOB: $('#txtSemiFOB').val(),
        SemiDuty: $('#txtSemiDuty').val(),
        LandedCost: $('#txtLandedCost').val(),

        Price1: $('#txtPrice1').val(),
        Price2: $('#txtPrice2').val(),
        Price3: $('#txtPrice3').val(),
        Price4: $('#txtPrice4').val(),

        Margin1: $('#txtMargin1').val(),
        Margin2: $('#txtMargin2').val(),
        Margin3: $('#txtMargin3').val(),
        Margin4: $('#txtMargin4').val(),

        CompleteFOB: $('#txtCompleteFOB').val(),
        CompleteDuty: $('#txtCompleteDuty').val(),
        LandedCostComplete: $('#txtLandedCostComplete').val(),

        CompletePrice1: $('#txtCompletePrice1').val(),
        CompletePrice2: $('#txtCompletePrice2').val(),
        CompletePrice3: $('#txtCompletePrice3').val(),
        CompletePrice4: $('#txtCompletePrice4').val(),

        CompleteMargin1: $('#txtCompleteMargin1').val(),
        CompleteMargin2: $('#txtCompleteMargin2').val(),
        CompleteMargin3: $('#txtCompleteMargin3').val(),
        CompleteMargin4: $('#txtCompleteMargin4').val(),
        LaborDutyVal: LaborDutyVal,
        LaborPenaltyVal: LaborPenaltyVal    ,
        LaborTariffVal: LaborTariffVal,



        Remark: $('#txtRemark').val()
    };
}

/**
 * Fill all labor controls from an object.
 */
function setLaborObject(obj) {

    //$('#ddlLaborLocation').val(obj.LaborLocation);
    var laborLocation = obj.VendorCode + '|' + obj.LaborLocation;
    $('#ddlLaborLocation option:selected').text(obj.VendorCode),
        $('#ddlProcessType').val(obj.ProcessType);

    $('#txtCastingLabor').val(obj.CastingLabor);
    $('#txtCastPcs').val(obj.CastPcs);
    $('#txtCFP').val(obj.CFP);
    $('#txtRhodium').val(obj.Rhodium);
    $('#txtAssembly').val(obj.Assembly);
    $('#txtSolder').val(obj.Solder);
    $('#txtTag').val(obj.Tag);
    $('#txtDiaHandling').val(obj.DiaHandling);
    $('#txtFinHandling').val(obj.FinHandling);
    $('#txtStamping').val(obj.Stamping);
    $('#txtModel').val(obj.Model);
    $('#txtCAM').val(obj.CAM);
    $('#txtGiftBox').val(obj.GiftBox);
    $('#txtTotalLabor').val(obj.TotalLabor);

    $('#ddlOtherHead1').val(obj.OtherHead1);
    $('#txtOtherCost1').val(obj.OtherCost1);
    $('#ddlOtherHead2').val(obj.OtherHead2);
    $('#txtOtherCost2').val(obj.OtherCost2);
    $('#ddlOtherHead3').val(obj.OtherHead3);
    $('#txtOtherCost3').val(obj.OtherCost3);

    $('#txtSemiFOB').val(obj.SemiFOB);
    $('#txtSemiDuty').val(obj.SemiDuty);
    $('#txtLandedCost').val(obj.LandedCost);

    $('#txtPrice1').val(obj.Price1);
    $('#txtPrice2').val(obj.Price2);
    $('#txtPrice3').val(obj.Price3);
    $('#txtPrice4').val(obj.Price4);

    $('#txtMargin1').val(obj.Margin1);
    $('#txtMargin2').val(obj.Margin2);
    $('#txtMargin3').val(obj.Margin3);
    $('#txtMargin4').val(obj.Margin4);

    $('#txtCompleteFOB').val(obj.CompleteFOB);
    $('#txtCompleteDuty').val(obj.CompleteDuty);
    $('#txtLandedCostComplete').val(obj.LandedCostComplete);

    $('#txtCompletePrice1').val(obj.CompletePrice1);
    $('#txtCompletePrice2').val(obj.CompletePrice2);
    $('#txtCompletePrice3').val(obj.CompletePrice3);
    $('#txtCompletePrice4').val(obj.CompletePrice4);

    $('#txtCompleteMargin1').val(obj.CompleteMargin1);
    $('#txtCompleteMargin2').val(obj.CompleteMargin2);
    $('#txtCompleteMargin3').val(obj.CompleteMargin3);
    $('#txtCompleteMargin4').val(obj.CompleteMargin4);

    $('#txtRemark').val(obj.Remark);
}

/**
 * Clear all labor controls.
 */
function clearLaborControls() {
    $('#ddlLaborLocation').val('');

    $('#txtCastingLabor, #txtCastPcs, #txtCFP, #txtRhodium, #txtAssembly, #txtSolder, #txtTag, #txtDiaHandling, #txtFinHandling, #txtStamping, #txtModel, #txtCAM, #txtGiftBox, #txtTotalLabor')
        .val('');

    $('#txtOtherHead1, #txtOtherCost1, #txtOtherHead2, #txtOtherCost2, #txtOtherHead3, #txtOtherCost3')
        .val('');


    $('#txtSemiFOB, #txtSemiDuty, #txtLandedCost, #txtPrice1, #txtPrice2, #txtPrice3, #txtMargin1, #txtMargin2, #txtMargin3')
        .val('');

    $('#txtCompleteFOB, #txtCompleteDuty, #txtLandedCostComplete, #txtCompletePrice1, #txtCompletePrice2, #txtCompletePrice3, #txtCompleteMargin1, #txtCompleteMargin2, #txtCompleteMargin3')
        .val('');

    $('#txtRemark').val('');
}

/************************************************************
 * LABOR SUBMIT → SAVE SKU MODULE
 ************************************************************/
$('#btnSubmit').on('click', function () {
    saveSkuModule();
});

/************************************************************
 *  Script Section 8 — SKU MODULE (Collecting All Tabs)
 *  ---------------------------------------------------------
 *  Contains:
 *   - skuModule structure
 *   - collectSkuInfo()
 *   - collectStoneInfo()
 *   - collectLaborInfo()
 *   - Tab switching logic
 ************************************************************/





/************************************************************
 * COLLECT SKU INFORMATION (Vendor Product + Metals + Findings)
 ************************************************************/
function collectSkuInfo() {
    var skuID = 0;
    var createdby = 1;
    var createdon = new Date();
    // ✅ Skip updating VendorProduct if on /SKU/Edit
    if (window.location.pathname.toLowerCase().includes("/sku/edit")) {
        // return; // exit early
        skuID = parseInt(skuModule.skuInfo.VendorProduct.skuId);
        createdby = parseInt(skuModule.skuInfo.VendorProduct.createdBy);
        createdon = skuModule.skuInfo.VendorProduct.createdOn;

    }
    if (window.location.pathname.toLowerCase().includes("/sku/Info")) {
        // return; // exit early
        skuID = parseInt(skuModule.skuInfo.VendorProduct.skuId);
        createdby = parseInt(skuModule.skuInfo.VendorProduct.createdBy);
        createdon = skuModule.skuInfo.VendorProduct.createdOn;

    }
    //else {
        skuModule.skuInfo.VendorProduct = {
            skuId: skuID,
            company: $('#ddlCompany').val(),
            vendor: $('#ddlVendor').val(),
            orderType: $('#ddlOrderType').val(),
            vendorNumber: $('#txtVendorNumber').val(),
            skuNumber: $('#txtSKUNumber').val(),
            collection: $('#ddlCollection option:selected').text().trim(),
            collectionCode: $('#ddlCollection').val().trim(),
            category: $('#ddlCategory option:selected').text().trim(),
            categoryCode: $('#ddlCategory').val().trim(),
            subCategory: $('#ddlSubCategory option:selected').text().trim(),
            subCategoryCode: $('#ddlSubCategory').val().trim(),
            sizeLength: $('#txtSizeLength').val(),
            mmWidth: $('#txtMMWidth').val(),
            mmHeight: $('#txtMMHeight').val(),
            semiMinWt: 0,
            centerMinWt: 0,
            SemiAdjWt: $('#SemiAdjWt').val(),//Added By Mahesh
            CenterAdjWt: $('#txtCenterAdjWt').val(),//Added By Mahesh
            createdBy: createdby,
            createdOn: createdon,


        };
    //}
    // Sync global arrays with module aliases
    findings = findingLines;
    metals = metalLines;

    skuModule.skuInfo.Metals = metals;
    skuModule.skuInfo.Findings = findings;
}

/************************************************************
 * COLLECT STONE INFORMATION
 ************************************************************/
function collectStoneInfo() {
    skuModule.stoneInfo = stoneList;
    semiMinWt = $('#txtSemiMinWt').val(); //totalSemiWt;
    centerMinWt = $('#txtCenterMinWt').val(); //totalCenterWt;
    SemiAdjWt = $('#txtSemiAdjWt').val();
    CenterAdjWt = $('#txtCenterAdjWt').val();
    skuModule.skuInfo.VendorProduct.semiMinWt = semiMinWt;
    skuModule.skuInfo.VendorProduct.centerMinWt = centerMinWt;
    skuModule.skuInfo.VendorProduct.SemiAdjWt = SemiAdjWt;
    skuModule.skuInfo.VendorProduct.CenterAdjWt = CenterAdjWt;
}

/************************************************************
 * COLLECT LABOR INFORMATION
 ************************************************************/
function collectLaborInfo() {
    skuModule.laborInfo = getLabor();
    //skuModule.calculations = {
    //    totalMetalCost: totalMetalCost,
    //    totalFindingCost: totalFindingCost,
    //    totalSemiStoneCost: totalSemiStoneCost,
    //    totalCenterStoneCost: totalCenterStoneCost,
    //    totalSemiSettingCost: totalSemiSettingCost,
    //    totalCenterSettingCost: totalCenterSettingCost,
    //    totalLaborCost: totalLaborCost,
    //    semiDuty: semiDuty
    //    //semiFOB: 0,
    //    //completeFOB: 0,
    //    //semiPrice1: 0,
    //    //semiPrice2: 0,
    //    //semiPrice3: 0,
    //    //semiPrice4: 0,
    //    //centerPrice1: 0,
    //    //centerPrice2: 0,
    //    //centerPrice3: 0,
    //    //centerPrice4: 0,
    //    //semiMargin1: 0,
    //    //semiMargin2: 0,
    //    //semiMargin3: 0,
    //    //semiMargin4: 0,
    //    //centerMargin1: 0,
    //    //centerMargin2: 0,
    //    //centerMargin3: 0,
    //    //centerMargin4: 0,
    //    //landedcost: 0,
    //    //landedcostCenter: 0
    //};
}

/************************************************************
 * SAVE DATA WHEN SWITCHING TABS
 ************************************************************/
function saveCurrentTabData(tabId) {
    switch (tabId) {
        case 'nav-sku-information':
            collectSkuInfo();
            if (path.includes("/sku/edit")) {
                fillLaborFOBValues();

            }
            break;

        case 'nav-stone-information':
            collectStoneInfo();
            fillLaborFOBValues();
            break;

        case 'nav-labor-information':
            collectLaborInfo();
            if (path.includes("/sku/edit")) {
                fillLaborFOBValues();

            }
            break;
    }

    console.log('Module state:', skuModule);
}

/************************************************************
 * TAB SWITCH EVENT
 ************************************************************/
$(function () {
    $('#nav-tab button[data-bs-toggle="tab"]').on('shown.bs.tab', function (event) {
        const prevPaneId = $(event.relatedTarget).attr('data-bs-target');
        const currentPaneId = $(event.target).attr('data-bs-target');
        if (currentPaneId === '#nav-sku-information') {
            const skupv = skuModule.skuInfo.VendorProduct;
            setSelectedText("ddlSubCategory", skupv.subCategory);
        }
        if (!prevPaneId) return;

        saveCurrentTabData(prevPaneId.substring(1)); // remove '#'
    });
});

/************************************************************
 * SAVE SKU MODULE (Called on Labor Submit)
 ************************************************************/
function saveSkuModule() {

    // Disable Summary tab until save completes
    $('#nav-summary-tab').addClass('disabled').attr('disabled', true);

    // Collect data before saving    
    collectLaborInfo();

    $.ajax({
        url: webRoot + '/api/sku/savesku',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(skuModule),

        success: function (response) {
            console.log("SKU saved successfully:", response);
            loadSummaryFromSkuModel(skuModule);
            // Enable Summary tab after save
            $('#nav-summary-tab').removeClass('disabled').attr('disabled', false);

            // Optionally auto-switch to Summary tab
            $('#nav-summary-tab').click();
            alert("SKU saved successfully.");
            $('#btnSubmit').addClass('disabled').attr('disabled', true);
        },

        error: function (xhr) {
            console.error("Save failed:", xhr);
            //{
            //    "Message": "An error has occurred.",
            //        "ExceptionMessage": "SKU Number already exists. Please use a different SKU Number.",
            //            "ExceptionType": "System.Exception",
            //                "StackTrace": "   at POEMPricing.API.SKUController.SaveSku(SkuModuleDto model) in E:\\POEM\\SourceCode\\PoemPricingV2\\POEMPricing\\API\\SKUController.cs:line 380"
            //}
            //if (xhr.responseJSON.ExceptionMessage)
            // Keep Summary disabled on failure
            alert("Error saving SKU. Please try again.");
        }
    });
}

function loadSummaryFromSkuModel(skuModel) {
    if (!skuModel || !skuModel.skuInfo) return;



    const vp = skuModel.skuInfo.VendorProduct;
    const metals = skuModel.skuInfo.Metals?.[0] || {};
    const stones = skuModel.stoneInfo?.[0] || {};
    const labor = skuModel.laborInfo || {};

    // Helper to set label text
    function setLabel(id, value) {
        const el = document.getElementById(id);
        if (el) el.textContent = value || "—";
    }

    // VendorProduct mapping
    setLabel("lblCompanyValue", vp.company);
    setLabel("lblOrderTypeValue", vp.orderType);
    setLabel("lblCollectionValue", vp.collection);
    setLabel("lblSkuValue", vp.skuNumber);
    setLabel("lblCategoryValue", vp.category);
    setLabel("lblSubCategoryValue", vp.subCategory);
    setLabel("lblSizeLengthValue", vp.sizeLength);
    SkuNumberPDF = vp.skuNumber;
    // Metals
    setLabel("lblRatePOzValue", '$ ' + metals.ratePOz);
    var metaltxtval = (metals.karatText.length > 0 ? metals.karatText + ' ' : '') + metals.colorText + ' ' + metals.metalText
    setLabel("lblMetalValue", metaltxtval);

    // Stones
    if (skuModel.stoneInfo && Array.isArray(skuModel.stoneInfo)) {
        // Total Qty across all stones
        const totalQty = skuModel.stoneInfo.reduce((sum, s) => sum + (parseInt(s.Qty, 10) || 0), 0);
        setLabel("lblNoOfStonesValue", totalQty);

        // Semi Wt = sum of TotalStoneWt where SettingLocation = 'Semi'
        const semiWt = skuModel.stoneInfo
            .filter(s => s.SettingLocation === "Semi")
            .reduce((sum, s) => sum + (parseFloat(s.TotalStoneWt) || 0), 0);
        setLabel("lblSemiWtValue", semiWt.toFixed(2));

        // Center Wt = sum of TotalStoneWt where SettingLocation = 'Center'
        const centerWt = skuModel.stoneInfo
            .filter(s => s.SettingLocation === "Center")
            .reduce((sum, s) => sum + (parseFloat(s.TotalStoneWt) || 0), 0);
        setLabel("lblCenterWtValue", centerWt.toFixed(2));

        // Complete Wt = Semi + Center
        const completeWt = semiWt + centerWt;
        setLabel("lblCompleteWtValue", completeWt.toFixed(2));

        // Stone qualities (optional: pick first or aggregate)
        const semiQuality = skuModel.stoneInfo.find(s => s.SettingLocation === "Semi")?.StoneQualityVal || "";
        const centerQuality = skuModel.stoneInfo.find(s => s.SettingLocation === "Center")?.StoneQualityVal || "";

        setLabel("lblSemiStoneQualityValue", semiQuality);
        setLabel("lblCenterStoneQualityValue", centerQuality);
    } else {
        setLabel("lblNoOfStonesValue", "—");
        setLabel("lblSemiWtValue", "—");
        setLabel("lblCenterWtValue", "—");
        setLabel("lblCompleteWtValue", "—");
    }



    // Prices (from laborInfo) - display as rounded up whole numbers with $ prefix
    setLabel("lblPrice1Value", '$ ' + Math.ceil(parseFloat(labor.Price1 || 0)));
    setLabel("lblPrice2Value", '$ ' + Math.ceil(parseFloat(labor.Price2 || 0)));
    setLabel("lblPrice3Value", '$ ' + Math.ceil(parseFloat(labor.Price3 || 0)));
}

/************************************************************
 *  Script Section 9 — API CALLS + FINAL WIRING
 *  ---------------------------------------------------------
 *  Contains:
 *   - toggleTableVisibility()
 *   - Any remaining API calls
 *   - Final event bindings
 ************************************************************/

/**
 * Toggle visibility of Findings and Metals tables.
 */
function toggleTableVisibility() {
    if ($('#tblFindings tbody tr').length === 0) {
        $('#tblFindings').hide();
    } else {
        $('#tblFindings').show();
    }

    if ($('#tblMetals tbody tr').length === 0) {
        $('#tblMetals').hide();
    } else {
        $('#tblMetals').show();
    }
}


/************************************************************
 * FINAL EVENT BINDINGS (Stone MM Size → Per Stone Weight)
 ************************************************************/

// Debounced API call for per-stone weight
const debouncedLoadPerStoneWeight = debounce(function () {
    const stoneType = $('#ddlStoneType').val();
    const growingType = $('#ddlGrowing').val();
    const stoneShape = $('#ddlStoneShape option:selected').text().trim();
    const lengthDiameter = $('#txtStoneMMSize').val();
    
    if (stoneType && growingType && stoneShape && lengthDiameter) {
        loadPerStoneWeight(stoneType, growingType, stoneShape, lengthDiameter);
        if ($('#txtStoneQty').val().length > 0) {
            $('#txtStoneQty').trigger('input'); // Trigger recalculation if quantity is present 
        }
    }
}, 400);

// Bind MM size input to debounced API call
$('#txtStoneMMSize').on('input', debouncedLoadPerStoneWeight);


// Main SKU module object
const skuModule = {
    skuInfo: {
        VendorProduct: {},
        Metals: [],
        Findings: []
    },
    stoneInfo: [],
    laborInfo: {},
    calculations: {}
};

document.addEventListener("DOMContentLoaded", function () {

    const path = window.location.pathname.toLowerCase();

    if (path.includes("/sku/edit")) {
        $('#nav-summary-tab').click();
        // ✅ Change button text to "Update" on edit page
        document.getElementById("btnSubmit").textContent = "Update";
        // ✅ overwrite global skuModule instead of shadowing
        Object.assign(skuModule, JSON.parse(document.getElementById("skuData").textContent));

        console.log(skuModule);

        if (skuModule && skuModule.skuInfo) {
            $('#btnSubmit').removeClass('disabled').attr('disabled', false);
            bindFormData(skuModule);
            loadSummaryFromSkuModel(skuModule);
            $('#nav-sku-information-tab').click();
            $('#nav-summary-tab').removeClass('disabled').attr('disabled', false);

        }
    }
    else {
        // Default text for other pages
        document.getElementById("btnSubmit").textContent = "Submit";
        $('#btnSubmit').removeClass('disabled').attr('disabled', false);

    }

    if (path.includes("/sku/info")) {

        // ✅ overwrite global skuModule instead of shadowing
        Object.assign(skuModule, JSON.parse(document.getElementById("skuData").textContent));

        console.log(skuModule);

        if (skuModule && skuModule.skuInfo) {
            $('#nav-summary-tab').click();
            $('#btnSubmit').addClass('disabled').attr('disabled', true);
            bindFormData(skuModule);
            loadSummaryFromSkuModel(skuModule);
            $('#nav-summary-tab').removeClass('disabled').attr('disabled', false);

        }
        $('input').prop('readonly', true);
        $('select').prop('disabled', true);
        //$('select').on('focus', function () {
        //    $(this).data('current-value', $(this).val());
        //}).on('change', function () {
        //    $(this).val($(this).data('current-value'));
        //});
        $('select option:not(:selected)').prop('disabled', true);
    }

    if (path.includes("/sku/copy")) {

        // ✅ overwrite global skuModule instead of shadowing
        Object.assign(skuModule, JSON.parse(document.getElementById("skuData").textContent));

        console.log(skuModule);

        if (skuModule && skuModule.skuInfo) {
            $('#nav-sku-information').click();
            bindFormData(skuModule);
            loadSummaryFromSkuModel(skuModule);
            $("#txtSKUNumber").val(""); // Clear SKU number for copy"
            $("#txtVendorNumber").val(""); // Clear Vendor Number for copy"
            skuModule.skuInfo.VendorProduct.skuId = 0; // Reset SKU ID for new SKU
            $('#nav-summary-tab').removeClass('disabled').attr('disabled', true);

        }
    }

});



function setValue(id, value) {
    var el = document.getElementById(id);
    if (el) {
        if (el.tagName === "SELECT") {
            //el.value = value; // set dropdown value

            //// ✅ Trigger change event
            //const event = new Event("change", { bubbles: true });
            //el.dispatchEvent(event);
            $("#" + id).val(value).change()
        } else {
            el.value = value; // textbox

            // ✅ Trigger input event for textboxes
            const event = new Event("input", { bubbles: true });
            el.dispatchEvent(event);
        }
    }
}

function setSelectedText(dropdownId, text) {
    const ddl = document.getElementById(dropdownId);
    if (!ddl) return;
    if (!text) return;
    const target = text.trim().toLowerCase();

    for (let i = 0; i < ddl.options.length; i++) {
        if (ddl.options[i].text.trim().toLowerCase() === target) {
            ddl.selectedIndex = i;
            //$('#' + dropdownId).val(text).trigger('change');
            ddl.dispatchEvent(new Event("change", { bubbles: true }));
            if (dropdownId === 'ddlCategory') {
                const parentId = $('#ddlCategory').val();

                $.get(webRoot + '/api/sku/subcategory?parentId=' + parentId, function (data) {
                    const subDropdown = $('#ddlSubCategory');
                    subDropdown.empty().append('<option value=""></option>');

                    $.each(data, function (i, item) {
                        subDropdown.append($('<option>').val(item.Key).text(item.Value));
                    });
                });
            }
            break;
        }
    }
}

// Bind form controls across tabs
function bindFormData(skuModel) {
    const vp = skuModel.skuInfo.VendorProduct;
    //const metals = skuModel.skuInfo.Metals?.[0] || {};
    const metals = skuModel.skuInfo.Metals || {};
    //const stones = skuModel.stoneInfo?.[0] || {};
    const stones = skuModel.stoneInfo || {};
    const labor = skuModel.laborInfo || {};
    const calculations = skuModel.calculations || {};



    // VendorProduct    
    setValue("ddlCompany", vp.company);
    setValue("ddlVendor", vp.vendor);
    setValue("txtVendorNumber", vp.vendorNumber);
    setValue("ddlOrderType", vp.orderType);
    //setSelectedText("ddlCollection", vp.collection);
    setValue("ddlCollection", vp.collectionCode);
    setValue("txtSKUNumber", vp.skuNumber);
    //setSelectedText("ddlCategory", vp.category);
    setValue("ddlCategory", vp.categoryCode);

    //setSelectedText("ddlSubCategory", vp.subCategory);
    setValue("ddlSubCategory", vp.subCategoryCode);

    setValue("txtSizeLength", vp.sizeLength);
    setValue("txtMMWidth", vp.mmWidth);
    setValue("txtMMHeight", vp.mmHeight);

    // Metals
    //metalLines = [metals]; // set global metals array
    metalLines = metals; // set global metals array
    renderMetalGrid();
    toggleTableVisibility();
    setValue("txtSemiMinWt", vp.semiMinWt);
    setValue("txtCenterMinWt", vp.centerMinWt);
    setValue("txtSemiAdjWt", vp.SemiAdjWt);
    setValue("txtCenterAdjWt", vp.CenterAdjWt);

    //Findings

    findingLines = skuModel.skuInfo.Findings || [];
    renderTable();
    toggleTableVisibility();
    // Stones
    stoneList = skuModel.stoneInfo || []; // set global stone array
    renderStoneTable();

    // Labor
    setSelectedText("ddlLaborLocation", labor.VendorName);
    setValue("ddlProcessType", labor.ProcessType);
    setValue("txtCastPcs", labor.CastPcs);
    setValue("txtGiftBox", labor.GiftBox);
    /*Added by Mahesh Start*/
    setValue("txtCFP", labor.CFP);
    setValue("txtRhodium", labor.Rhodium);
    setValue("txtAssembly", labor.LaborAssembly);
    setValue("txtTag", labor.Tag);
    setValue("txtDiaHandling", labor.DiaHandling);
    //setValue("ddlLaborLocation", labor.LaborLocation);
    setValue("txtCastingLabor", labor.CastingLabor);
    setValue("txtModel", labor.Model);



    totalSemiAdjWt = parseFloat(vp.SemiAdjWt) || 0;
    totalCenterAdjWt = parseFloat(vp.CenterAdjWt) || 0;
    totalCenterWt = parseFloat(vp.centerMinWt) || 0;
    totalSemiWt = parseFloat(vp.semiMinWt) || 0;
    /*Added by Mahesh End*/

    // Calculations
    totalMetalCost = calculations.totalMetalCost || 0;
    totalFindingCost = calculations.totalFindingCost || 0;
    totalSemiStoneCost = calculations.totalSemiStoneCost || 0;
    totalCenterStoneCost = calculations.totalCenterStoneCost || 0;
    totalSemiSettingCost = calculations.totalSemiSettingCost || 0;
    totalCenterSettingCost = calculations.totalCenterSettingCost || 0;
    totalLaborCost = calculations.totalLaborCost || 0;
    semiDuty = calculations.semiDuty || 0;
    getMarginDetails();
    //setTimeout(fillLaborFOBValues, 1100);
    //fillLaborFOBValues();//
}

// Bind summary labels
function bindSummaryData(skuModel) {
    const vp = skuModel.skuInfo.VendorProduct;
    const metals = skuModel.skuInfo.Metals?.[0] || {};
    const stones = skuModel.stoneInfo?.[0] || {};
    const labor = skuModel.laborInfo || {};
    const calculations = skuModel.calculations || {};



    function setLabel(id, value) {
        var el = document.getElementById(id);
        if (el) el.textContent = value || "—";
    }

    setLabel("lblCompanyValue", vp.company);
    setLabel("lblOrderTypeValue", vp.orderType);
    setLabel("lblCollectionValue", vp.collection);
    setLabel("lblSkuValue", vp.skuNumber);
    setLabel("lblCategoryValue", vp.category);
    setLabel("lblSubCategoryValue", vp.subCategory);
    setLabel("lblSizeLengthValue", vp.sizeLength);

    setLabel("lblRatePOzValue", metals.ratePOz);
    setLabel("lblMetalValue", metals.metalText);

    setLabel("lblNoOfStonesValue", stones.Qty);
    setLabel("lblSemiWtValue", stones.SemiMinWt);
    setLabel("lblCenterWtValue", stones.CenterMinWt);
    setLabel("lblCompleteWtValue", stones.TotalMinWt);
    setLabel("lblSemiStoneQualityValue", stones.StoneQuality);
    setLabel("lblCenterStoneQualityValue", stones.CenterStoneQuality || "");

    // Bind summary labels: rounded up whole numbers (no decimals)
    setLabel("lblPrice1Value", '$ ' + Math.ceil(parseFloat(labor.Price1 || 0)));
    setLabel("lblPrice2Value", '$ ' + Math.ceil(parseFloat(labor.Price2 || 0)));
    setLabel("lblPrice3Value", '$ ' + Math.ceil(parseFloat(labor.Price3 || 0)));
}
// Helper: Base64 encode
function encryptSku(value) {
    return btoa(value); // encodes string to Base64
}
$("#btnEdit").on("click", function () {
    // Get SKU number from summary label
    var skuNumber = $("#lblSkuValue").text().trim();

    if (skuNumber) {
        // Redirect to Edit page with query string
        const encryptedSku = encryptSku(skuNumber.trim());
        window.location.href = "/SKU/Edit?skunumber=" + encodeURIComponent(encryptedSku);
    } else {
        alert("SKU number not found in summary.");
    }
});

$("#btnCopy").on("click", function () {
    // Get SKU number from summary label
    var skuNumber = $("#lblSkuValue").text().trim();

    if (skuNumber) {
        // Redirect to Edit page with query string
        const encryptedSku = encryptSku(skuNumber.trim());
        window.location.href = "/SKU/Copy?skunumber=" + encodeURIComponent(encryptedSku);
    } else {
        alert("SKU number not found in summary.");
    }
});


const txtCastPcs = document.getElementById("txtCastPcs");
const ddlLaborLocation = document.getElementById("ddlLaborLocation");
const ddlCategory = document.getElementById("ddlCategory");
const txtCFP = document.getElementById("txtCFP");
const txtRhodium = document.getElementById("txtRhodium");
const txtAssembly = document.getElementById("txtAssembly");
const txtSolder = document.getElementById("txtSolder");


function GetCFP() {
    const castPcs = parseInt(txtCastPcs.value, 10);

    //// Only allow 1 or 2
    //if (![1, 2].includes(castPcs)) {
    //    txtCFP.value = "";
    //    return;
    //}

    const vendorCode = $('#ddlLaborLocation').val();
    const category = $('#ddlCategory option:selected').text().trim();

    //// Find CFP record
    //const record = msdCFPLaborData.find(r =>
    //    r.VendorCode === vendorCode.trim()
    //    && r.Category === category.trim()
    //    && r.LaborType ==="CFP"
    //);

    //if (record) {
    //    if (castPcs === 1) {
    //        txtCFP.value = record.GoldCast1.toFixed(2);
    //    } else if (castPcs === 2) {
    //        txtCFP.value = record.GoldCast2.toFixed(2);
    //    }            
    //} else {
    //    txtCFP.value = "";            
    //}

    //// Find Rhodium record
    //const Rhodiumrecord = msdCFPLaborData.find(r =>
    //    r.VendorCode === vendorCode.trim()
    //    && r.Category === category.trim()
    //    && r.LaborType === "Rhodium"
    //);

    //if (Rhodiumrecord) {
    //    if (castPcs === 1) {
    //        txtRhodium.value = Rhodiumrecord.GoldCast1.toFixed(2);
    //    } else if (castPcs === 2) {
    //        txtRhodium.value = Rhodiumrecord.GoldCast2.toFixed(2);
    //    }
    //} else {
    //    txtRhodium.value = "";
    //}

    //// Find Assembly record
    //const Assemblyrecord = msdCFPLaborData.find(r =>
    //    r.VendorCode === vendorCode.trim()
    //    && r.Category === category.trim()
    //    && r.LaborType === "Assembly"
    //);

    //if (Assemblyrecord) {
    //    if (castPcs === 1) {
    //        txtAssembly.value = Assemblyrecord.GoldCast1.toFixed(2);
    //    } else if (castPcs === 2) {
    //        txtAssembly.value = Assemblyrecord.GoldCast2.toFixed(2);
    //    }
    //} else {
    //    txtAssembly.value = "";
    //}

    // Find Solder record
    //const Solderrecord = msdCFPLaborData.find(r =>
    //    r.VendorCode === vendorCode.trim()
    //    && r.Category === category.trim()
    //    && r.LaborType === "Solder"
    //);

    //if (Solderrecord) {
    //    if (castPcs === 1) {
    //        txtSolder.value = Solderrecord.GoldCast1.toFixed(2);
    //    } else if (castPcs === 2) {
    //        txtSolder.value = Solderrecord.GoldCast2.toFixed(2);
    //    }
    //} else {
    //    txtSolder.value = "";
    //}

    // Find Dia. Handling
    // Need business rules for Dia. Handling lookup
}
// Attach event listeners to trigger calculation on change
["txtCFP", "txtCAM", "txtModel", "txtGiftBox", "txtOtherCost1", "txtOtherCost2", "txtOtherCost3",'txtRhodium', "txtDiaHandling", 'txtFinHandling', 'txtStamping'].forEach(id => {
    const el = document.getElementById(id);
    if (el) {
        el.addEventListener("input", function () {
            //if (id === "txtCastPcs") {
            //    GetCFP(); // call CFP lookup
            //}
            calculateTotalLabor(); // always recalc labor
        });
        el.addEventListener("change", function () {
            //if (id === "txtCastPcs") {
            //    GetCFP(); // call CFP lookup
            //}
            calculateTotalLabor(); // always recalc labor
        });
        el.addEventListener("blur", function () {
            //if (id === "txtCastPcs") {
            //    GetCFP(); // call CFP lookup
            //}
            calculateTotalLabor(); // always recalc labor
        });
    }
});

const el = document.getElementById('txtTotalLabor');
if (el) {
    el.addEventListener("input", function () {
        if ($('#ddlProcessType').val() === 'Flat Labour per piece' || $('#ddlProcessType').val() === 'Flat Labour per gm') {
            totalLaborCost = parseFloat($(this).val()) || 0;
            totalLaborCostWTDuty = 0;
            fillLaborFOBValues(); // always recalc labor
        }
    });
}
const txtSemiMinWt = document.getElementById("txtSemiMinWt");
const txtSemiAdjWt = document.getElementById("txtSemiAdjWt");
const txtCenterMinWt = document.getElementById("txtCenterMinWt");
const txtCenterAdjWt = document.getElementById("txtCenterAdjWt");
const txtTotalMinWt = document.getElementById("txtTotalMinWt");
const txtTotalAdjWt = document.getElementById("txtTotalAdjWt");

function calculateTotals() {
    const semiMin = parseFloat(txtSemiMinWt.value) || 0;
    const semiAdj = parseFloat(txtSemiAdjWt.value) || 0;
    const centerMin = parseFloat(txtCenterMinWt.value) || 0;
    const centerAdj = parseFloat(txtCenterAdjWt.value) || 0;

    txtTotalMinWt.value = (semiMin + centerMin).toFixed(3); // keep 4 decimals
    txtTotalAdjWt.value = (semiAdj + centerAdj).toFixed(3);
}

// Attach events to recalc on input/blur
[txtSemiMinWt, txtSemiAdjWt, txtCenterMinWt, txtCenterAdjWt].forEach(el => {
    el.addEventListener("input", calculateTotals);
    el.addEventListener("blur", calculateTotals);
});

$(document).ready(function () {


    /* validate Tab Mahesh*/
    $('.nav-tabs button').on('show.bs.tab', function (e) {

        if (!validateTabs(e.relatedTarget.id, e.target.id)) {
            e.preventDefault();
        }
        //alert('New tab will be visible now!');
        //alert(e.relatedTarget.id);

    });
});

function validateTabs(tabname, newTab) {
    var validate = true;
   
    var ValidateField = ["#", "#"];

    if (tabname === "nav-sku-information-tab") {
        ValidateField = ["ddlCompany", "ddlVendor", "ddlOrderType", "ddlCategory", "ddlSubCategory", , "txtSKUNumber"];
    } else if (tabname === "nav-stone-information-tab") {
        //ValidateField = ["ddlStoneVendor", "ddlStoneType", "ddlGrowing", "ddlSettingLocation", "ddlStoneShape", "txtStoneMMSize", "txtStoneQty", "txtTotalAdjStoneWt","ddlStoneQuality"]
        validateCenterAdj();
        validateSemiAdj();
        var stoneAdded = $("#tblStone tr").length;
        if (stoneAdded > 1) {
            clearFieldError($("#tblStone"));
            validate= true;
        } else {
            setFieldError($("#tblStone"), "Please add Stone information");
            validate= false;
        }
        ValidateField = [];

        if ($('#txtSemiAdjWt').hasClass('is-invalid')) {
            validate = false;
        } if ($('#txtCenterAdjWt').hasClass('is-invalid')) {
            validate = false;
        }
    } else if (tabname === "nav-labor-information-tab") {
        //ValidateField = ["txtCastPcs"]
        return true;
    } else {
        return true;
    }
    ValidateField.forEach((name, index) => {

        const $el = $("#" + name);
        const validator = FieldValidators["#" + name];
        const error = validator($el);

        if (error) {
            setFieldError($el, error);
            validate = false;
        } else {
            clearFieldError($el);
        }
    });

    if (tabname === "nav-stone-information-tab" && newTab === "nav-sku-information-tab") {
        return validate;
    } else if (tabname === "nav-labor-information-tab" && (newTab === "nav-sku-information-tab" || newTab === "nav-stone-information-tab")) {
        return validate;
    } else if (tabname === "nav-summary-tab") {
        return validate;
    }

    return validate;


}



/************************************************************
 * Validate button  Mahesh
 ************************************************************/
function validateButtonAddUpdate(buttonid) {
    var validate = true;
    var ValidateField = [];
    if (buttonid === "btnMetalAddUpdate") {
        ValidateField = ["#ddlMetal", "#ddlKarat", "#ddlMetalColor", "#txtMetalGmWt"];
    } else if (buttonid === "btnFindingAddUpdate") {
        ValidateField = ["#ddlFindingSupplier", "#txtFindingSku", "#ddlFindingAssembly", "#txtFindingQty"];
    } else if (buttonid === "btnStoneAddUpdate") {
        ValidateField = ["#ddlStoneVendor", "#ddlStoneType", "#ddlGrowing", "#ddlSettingLocation", "#ddlStoneShape", "#txtStoneMMSize", "#txtStoneQty", "#txtTotalAdjStoneWt", "#ddlStoneQuality", "#ddlSettingVendor", "#ddlSettingType"];
        //ValidateField = ["#ddlSettingVendor", "#ddlSettingType"];
    } else { return true; }
    ValidateField.forEach((name, index) => {

        const $el = $(name);
        const validator = FieldValidators[name];
        const error = validator($el);

        if (error) {
            setFieldError($el, error);
            validate = false;
        } else {
            clearFieldError($el);
        }

        if (name === '#txtTotalAdjStoneWt') {

            validateTotalAdj();
            if ($('#txtTotalAdjStoneWt').hasClass('is-invalid')) {
                validate = false;
            }
        }
    });



    return validate;

}
function printSummaryPage() {
    $("#printSummary").html($("#divSummary").html());
    $("#printImage").html($("#divImage").html());
    $("#printPricing").html($("#divPricing").html());
    $("#divRmImage", $("#printSummarypg")).css('display', 'none');
    $("#divRmImage1", $("#printSummarypg")).css('display', 'none');
    var printContents = $("#printSummarypg").html();
    $("#printSummarypg").css('display', 'none');
    var printWindow = window.open('', "myWindow",
        "resizable=yes");
    printWindow.document.write('<html><head><title>Print</title><link href="/Content/bootstrap.css" rel="stylesheet"><style>.col-sm-6 { width: 30%;} .col-9 { width: 60%;} .col-3 { width: 35%;}</style></head><body>' + printContents + '</body></html>');
    printWindow.document.close();
    // Wait for content to load
    printWindow.onload = function () {
        printWindow.focus();
        printWindow.print();

        // Auto close after print dialog is closed
        printWindow.onafterprint = function () {
            printWindow.close();
        };
    };
    return false;

}


function downloadSummaryPage() {
    $("#printSummary").html($("#divSummary").html());
    $("#printImage").html($("#divImage").html());
    $("#printPricing").html($("#divPricing").html());
    $("#divRmImage", $("#printSummarypg")).css('display', 'none');;
    $("#divRmImage1", $("#printSummarypg")).css('display', 'none');;
    var printContents = $("#printSummarypg");
    const element = document.getElementById('printSummarypg');


    html2canvas(element, {
        useCORS: true, // Needed if your div contains images from other domains
    }).then(canvas => {



        const imgWidth = 210;
        const pageHeight = 297;
        const imgHeight = canvas.height * imgWidth / canvas.width;

        // Convert the canvas to an image data URL
        const imgData = canvas.toDataURL('image/png');


        // Initialize jsPDF
        //const { jsPDF } = window.jspdf;
        // 'p' for portrait, 'mm' for millimeters unit
        //const doc = new jsPDF('p', 'mm', 'a4');
        const doc = new jsPDF('p', 'mm', 'a4');
        // Calculate the dimensions to fit the PDF
        const pdfWidth = doc.internal.pageSize.getWidth();
        const pdfHeight = doc.internal.pageSize.getHeight();
        //const imgWidth = canvas.width;
        //const imgHeight = canvas.height;
        const ratio = Math.min(pdfWidth / imgWidth, pdfHeight / imgHeight);
        const width = imgWidth * ratio;
        const height = imgHeight * ratio;
        document.body.appendChild(canvas);
        // Add the image to the PDF
        doc.addImage(imgData, 'PNG', 0, 0, imgWidth, imgHeight);

        // Save the PDF file with a specific name
        let filename = 'downloaded-SKU-' + SkuNumberPDF + '.pdf';
        doc.save(filename);
        document.body.appendChild(canvas);
    });
    $("#printSummarypg").css('display', 'none');

}

function loadImage(filePath) {
    //$('#imgLoader').show();
    //$('#imgError').hide();
    //$('#spImage').hide();
    filePath = filePath + '.JPG';
    $.ajax({
        url: '/api/sharepoint/image',
        type: 'GET',
        data: { filePath: filePath },
        xhrFields: { responseType: 'blob' },  // important — treat response as binary
        success: function (blob) {
            var objectUrl = URL.createObjectURL(blob);
            $('#product_image')
                .attr('src', objectUrl)
                .show();
            $('#sumproduct_image')
                .attr('src', objectUrl)
                .show();
           /* $('#imgLoader').hide();*/
        },
        error: function (error) {
            var errorMessage = error;
            //$('#imgLoader').hide();
            //$('#imgError').show();
        }
    });
}

function getTaxDetails(taxtype, location='') {

    const vendorlocation = $('#ddlVendor').val();
   
   
    var  url = webRoot + '/api/sku/taxdetails/' + encodeURIComponent(taxtype) + '/' + encodeURIComponent(vendorlocation);
    var taxvalue = 0.0;
    if (taxtype != 'vendor') {
        url = url + '?location=' + location;
    }
    var DutyPer = 0.00;
    var TariffPer =0.00;
    var PenaltyPer = 0.00;
    var returndata = {
        DutyPer: 0.00,
        TariffPer: 0.00,
        PenaltyPer: 0.00
    };
    $.getJSON(url, function (data) {
        if (!data) return;
        returndata = data;
        if (data.Duty) {
            DutyPer = parseFloat(data.DutyPer) / 100;
        }
        if (data.Tariff) {
            TariffPer = parseFloat(data.TariffPer) / 100;
        }
        if (data.Penalty) {
            PenaltyPer = parseFloat( data.PenaltyPer) / 100;
        }
        taxvalue = taxvalue / 100;
        console.log(data);
        console.log(taxvalue);
        if (taxtype === 'vendor') {
            vendorDutyper = DutyPer;
            vendorTariffPer = TariffPer;
            vendorPenaltyPer = PenaltyPer;
        } else if(taxtype === 'diamond'){
            diamondDutyper = DutyPer;
            diamondTariffPer = TariffPer;
            diamondPenaltyPer = PenaltyPer;
        } else if(taxtype === 'labor'){
            laborDutyper = DutyPer;
            laborTariffPer = TariffPer;
            laborPenaltyPer = PenaltyPer;
            fillLaborFOBValues();
        } else if(taxtype === 'finding'){
            findingDutyper = DutyPer;
            findingTariffPer = TariffPer;
            findingPenaltyPer = PenaltyPer;
        } else if(taxtype === 'setting'){
            settingDutyper = DutyPer;
            settingTariffPer = TariffPer;
            settingPenaltyPer = PenaltyPer;
        }
        return returndata;
    });
}

$(function () {
    // adjust selectors to match your actual element IDs/classes
    var $semiMin = $('#SemiMinWt');
    var $semiAdj = $('#SemiAdjWt');
    var $centreMin = $('#CentreMinWt');
    var $centreAdj = $('#CentreAdjWt');

    if ($semiMin.length) {
        $semiMin.on('input change', function () {
            $semiAdj.trigger('change');
        });
    }

    if ($centreMin.length) {
        $centreMin.on('input change', function () {
            $centreAdj.trigger('change');
        });
    }
});


$(function () {
    // replace '#Shape' and the field IDs below with your actual selectors
    $('#ddlStoneShape').on('change', function () {
        var fieldsToClear1 = [
            '#ddlSizeRange',
            '#txtStoneMMSize',
            '#txtStoneWidth1',
            '#txtStoneWidth2',
            '#txtStoneQty',
            '#txtTotalStoneWt',
            '#txtTotalAdjStoneWt',
            '#ddlStoneQuality',
            '#txtStoneCostPerCarat',
            '#txtStoneTotalCost'
        ];
        if ($('#btnStoneAddUpdate').text() === 'Update Stone')
            return;

        fieldsToClear(fieldsToClear1);
    });

    $('#txtStoneMMSize').on('change input', function () {
        var fieldsToClear1 = [
            '#txtStoneWidth1',
            '#txtStoneWidth2',
            '#txtStoneCostPerCarat',
            '#txtStoneTotalCost'
        ];
        if ($('#btnStoneAddUpdate').text() ==='Update Stone')
            return;

        fieldsToClear(fieldsToClear1);
    });
    $('#ddlProcessType').on('change', function () {
        var fieldsToClear1 = [
            '#txtCFP',
            '#txtRhodium',
            '#txtDiaHandling',
            '#txtFinHandling',
            '#txtStamping',
            '#txtModel',
            '#txtCAM',
            '#txtGiftBox',
            '#txtTotalLabor',
            '#ddlOtherHead1',
            '#ddlOtherHead2',
            '#ddlOtherHead3',
            '#txtOtherCost1',
            '#txtOtherCost2',
            '#txtOtherCost3'
        ];
       
        fieldsToClear(fieldsToClear1);
    });
});
function fieldsToClear(fields) {
    fields.forEach(function (sel) {
        var $el = $(sel);
        if (!$el.length) return;
        if ($el.is(':checkbox') || $el.is(':radio')) {
            $el.prop('checked', false);
        } else {
            $el.val('');
        }
        // $el.trigger('change'); // propagate change in case other logic depends on it
    });
}