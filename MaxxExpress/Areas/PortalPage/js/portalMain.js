
$.noConflict();


jQuery(document).ready(function ($) {

    "use strict";

    [].slice.call(document.querySelectorAll('select.cs-select')).forEach(function (el) {
        new SelectFx(el);
    });

    /*Global Variables */
    var datetime = null, date = null, global_last_idle_time = null, global_idle_counter = null, view_notes_toggle = false, create_note_toggle = false, create_note_CreateAlert_toggle = false;


    //Date and Time on Index update function
    var updateCounter_fuctions = function () {
        date = moment(new Date())
        datetime.html(date.format('MMMM Do, YYYY, h:mm a'));
        global_idle_counter = global_idle_counter + 1;

        if (global_idle_counter > 480) // 8mins - Show Expire Warning
        {
            showExpireWarning();
        }
        if (global_idle_counter > 600) // 10mins - Show Logged Out Window
        {
            forceLogout();
        }
    };

    //Date and Time on Index function
    jQuery('.selectpicker').selectpicker;
    datetime = $('#dateandtime')
    updateCounter_fuctions();
    setInterval(updateCounter_fuctions, 1000);

    function showExpireWarning() {
        $("#session-expire-warning-modal").modal('show');
    }
    function forceLogout() {
        $.ajax({
            url: '/portal/logout',
            method: 'GET',
            success: function (data) {
                window.location.href = "../portal/logout";
            }
        });
    }

    //Delay Function 
    var delay = (function () {
        var timer = 0;
        return function (callback, ms) {
            clearTimeout(timer);
            timer = setTimeout(callback, ms);
        };
    })();

    /*--- Page Intialization ---*/
    $(".admin-resetStatus-dd").find('.cs-placeholder').html('Choose Reset Status');
    $('.load-action-dd').css('z-index', '5');
    $('.fuel-action-dd').css('z-index', '4');
    /*--- End Intialization ---*/

    $('.view-notes').click(function (e) {
        view_notes_toggle ^= true;

        if (view_notes_toggle == 1) {
            null;
        }
        if (view_notes_toggle == 0) {
            $('.message-content-item').removeClass('show'); //Toggle Off/Hide 
        }

        $('.create-note-drawer').removeClass('active'); //Hide Create note

        $('.notes-drawer').toggleClass('active');
        e.preventDefault();
    });

    $('.create-note').click(function (e) {
        create_note_toggle ^= true;

        if (create_note_toggle == 1) {
            $('.message-content-item').removeClass('show'); //Toggle Off/Hide 
        }
        if (create_note_toggle == 0) {
            $('.message-content-item').removeClass('show'); //Toggle Off/Hide 
        }

        $('.notes-drawer').removeClass('active'); //Hide Views note
        $('.create-note-drawer').toggleClass('active');
        e.preventDefault();
    });

    $('#btn-create-alert').on('click', function (event) {
        create_note_CreateAlert_toggle ^= true;

        if (create_note_CreateAlert_toggle == 1) {
            $('#vM_CreateNote_Alert_Start_Dt').removeAttr('disabled');
            $('#vM_CreateNote_Alert_End_Dt').removeAttr('disabled');
            $('#vM_CreateNote_Alert_IND').val('1');
            $('#btn-create-alert').removeClass('btn-outline-warning').addClass('btn-warning').html('Set Alert Dates');

        }
        if (create_note_CreateAlert_toggle == 0) {
            $('#vM_CreateNote_Alert_Start_Dt').val('').attr('disabled', true);
            $('#vM_CreateNote_Alert_End_Dt').val('').attr('disabled', true);
            $('#vM_CreateNote_Alert_IND').val('0');
            $('#btn-create-alert').removeClass('btn-warning').addClass('btn-outline-warning').html('Create Alert');
        }
    });

    //Show Note Modal 
    $('.note-remove-modal').on('click', function (event) {
        var noteID = '';
        noteID = $(this).attr("data-noteid");
        $('#noteDeleteModal').modal('show');
        $('#delete-note-nbr').html(noteID);
    });

    //Show Alert Modal 
    $('.note-remove-alert').on('click', function (event) {
        var noteID = '';
        noteID = $(this).attr("data-noteid");
        $('#alertDeleteModal').modal('show');
        $('#delete-alert-nbr').html(noteID);
    });

    //Show Archive Modal 
    $('.note-archive').on('click', function (event) {
        var noteID = '';
        noteID = $(this).attr("data-noteid");
        $('#noteArchiveModal').modal('show');
        $('#archive-note-nbr').html(noteID);
    });

    //Deactivate Note Method 
    $('#btn-delete-note').on('click', function (event) {
        var note_ID = '';
        note_ID = $('#delete-note-nbr').text();

        var editType = '';
        editType = "DeleteNote";

        $.ajax({
            method: "PUT",
            url: '../portal/EditNote?NoteID=' + note_ID + '&EditType=' + editType + '',
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);

                    resetAction();
                    reloadIndex();
                    reloadNotesAlerts();
                    $('.message-content-item').removeClass('show'); //Toggle Off/Hide 
                    $('#noteDeleteModal').modal('hide');
                    $('.note-delete-successful').removeClass('hide');

                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);

            }

        });
    });

    //Deactivate Alert Method 
    $('#btn-delete-alert').on('click', function (event) {
        var note_ID = '';
        note_ID = $('#delete-alert-nbr').text();

        var editType = '';
        editType = "DeleteAlert";

        $.ajax({
            method: "PUT",
            url: '../portal/EditNote?NoteID=' + note_ID + '&EditType=' + editType + '',
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);

                    resetAction();
                    reloadIndex();
                    reloadNotesAlerts();
                    $('.message-content-item').removeClass('show'); //Toggle Off/Hide 
                    $('#alertDeleteModal').modal('hide');
                    $('.alert-delete-successful').removeClass('hide');

                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);

            }

        });
    });

    //Archive Note Method 
    $('#btn-archive-note').on('click', function (event) {
        var note_ID = '';
        note_ID = $('#archive-note-nbr').text();

        var editType = '';
        editType = "ArchiveNote";

        $.ajax({
            method: "PUT",
            url: '../portal/EditNote?NoteID=' + note_ID + '&EditType=' + editType + '',
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);

                    resetAction();
                    reloadIndex();
                    reloadNotesAlerts();
                    $('.message-content-item').removeClass('show'); //Toggle Off/Hide 
                    $('#noteArchiveModal').modal('hide');
                    $('.note-archive-successful').removeClass('hide');

                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);

            }

        });
    });

    $('#menuToggle').on('click', function (event) {
        $('body').toggleClass('open');
        $('#main-menu').toggleClass('padding-top-xl');
        $('.smallmaxxlogo').toggle('hide');
        $('.smallmlogo').toggle('show');
        $('#maxx-txt-logo').toggle('show');
        $('.Hello-Txt').toggle('hide');
    })

    $('.search-trigger').on('click', function (event) {
        event.preventDefault();
        event.stopPropagation();
        $('.search-trigger').parent('.header-left').addClass('open');
    });

    $('.search-close').on('click', function (event) {
        event.preventDefault();
        event.stopPropagation();
        $('.search-trigger').parent('.header-left').removeClass('open');
    });

    var moneyRoundClasses = document.getElementsByClassName("money-round");
    for (var i = 0; i < moneyRoundClasses.length; i++) {
        var amount = parseInt(moneyRoundClasses[i].innerText);
        moneyRoundClasses[i].innerHTML = "$" + amount.toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    }
    var moneyClasses = document.getElementsByClassName("money");
    for (var i = 0; i < moneyClasses.length; i++) {
        var amount = parseFloat(moneyClasses[i].innerText);
        moneyClasses[i].innerHTML = "$" + amount;
    }

    // initalizePageLoad();

    function initalizePageLoad() {
        $('[data-toggle="popover"]').popover();
    }

    //Expense Page on Load
    function updateExpenseData() {
        var totals = document.querySelector('[data-datetext]');
        $('#expenseDateText').text($(totals).attr('data-datetext'));
        $('#expenseTotal').text($(totals).attr('data-total'));
    }
    //Expense Page


    //Create Note - Submit 
    $('#btn-send-note').on('click', function (event) {

        var sourceForm = '';
        var disabled = null;
        var dataset = null;

        sourceForm = $('.createNoteform');
        disabled = sourceForm.find(':input:disabled').removeAttr('disabled');
        dataset = JSON.stringify($('.createNoteform').serializeCreateNote());

        $.ajax({
            method: "POST",
            url: '../portal/CreateNote',
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            data: dataset,
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);
                    disabled.attr('disabled', 'disabled');
                    $('.create-note-drawer').removeClass('active'); //Hide Create note
                    $('.note-send-successful').removeClass('hide');

                    reloadNotesAlerts();

                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);
            }

        });
    });


    //Get Open Load Detail Method
    $('#open-load-row').on('click', '.loadnumber_cl', function (event) {
        $("#loadsdtl").html('<div class="loading-message"></div>').css('min-height', '100px');
        var load_num = $(this).text();
        $("#loadsdtl").load('../portal/GetLoadDetail?LoadNbr=' + load_num + '&LoadState=open');
        $('#loadDetailModal').modal('show');

    });

    //Get Closed Load Detail Method
    $('#closed-load-row').on('click', '.closedloadnumber_cl', function (event) {
        $("#loadsdtl").html('<div class="loading-message"></div>').css('min-height', '100px');
        var load_num = $(this).text();
        $("#loadsdtl").load('../portal/GetLoadDetail?LoadNbr=' + load_num + '&LoadState=closed');
        $('#loadDetailModal').modal('show');

    });

    //Get Load Edit Method
    $('.editthisLoad_btn').on('click', function (event) {
        var load_num = $('input[name="editThisLoad_chk"]:checked').val();
        $("#openloadsEdit").load('../portal/GetLoadEditDetail?LoadNbr=' + load_num);
        $("#loadEditListModal").modal("show");
        $('#loadEditModal').modal({ backdrop: 'static', keyboard: false, show: true });
        $('#edit-load-success').addClass('hide');
    });

    //Edit Load from menu - Get List of Loads to edit
    $('.edit-load-btn').on('click', function (event) {
        $('#edit-load-success').addClass('hide');
        $("#openloadsEditList").load('../portal/GetLoadChangeList?ChangeAction=edit');
        $('#loadEditListModal').modal('show');

    });

    //Add Load Button from Driver View - Launch Modal and Populate Drop Down Values
    $('.add-load-btn_driver').on('click', function (event) {
        $('#Conf_Ind').val('0').removeClass('fileInput-maxx-validation');
        $('#BOL_Ind').val('0').removeClass('fileInput-maxx-validation');
        $("#openloadsAdd").load('../portal/GetAddLoad', function () { createTempLoadNbr() });
        $('#loadAddModal').modal({ backdrop: 'static', keyboard: false, show: true });
        $('.add-multi-dropoff-btn').removeClass('hide');

    });

    //Add Fuel Button from Driver View - Launch Modal and Populate Drop Down Values
    $('.add-fuel-btn_driver').on('click', function (event) {
        $('#Receipt_Ind').val('0').removeClass('fileInput-maxx-validation');
        $("#fuelPurchaseAdd").load('../portal/GetAddFuel', function () { null });
        $('#fuelAddModal').modal({ backdrop: 'static', keyboard: false, show: true });
    });

    //Delete Fuel Button from Driver View - Launch Modal and Populate Drop Down Values
    $('.delete-fuel-btn_driver').on('click', function (event) {
        $('#fuelDeleteListModal').removeClass('hide');
        $("#openfuelDeleteList").load('../portal/GetFuelPurchasesList', function () { null });
        $('#fuelDeleteListModal').modal({ backdrop: 'static', keyboard: false, show: true });
        $('.fuel-edit-title-text').text('Delete');
        $('.markaspaidthisFuelPurchase_btn').addClass('hide');
        $('.deletethisFuelPurchase_btn').removeClass('hide');
    });

    //Final Fuel Delete Oppourtunity
    $('.deletethisFuelPurchase_btn').on('click', function (event) {
        var fID = $('input[name="ThisFuelPurchase_chk"]:checked').val();
        $('#delete-fuel-id').text(fID);
        $('#loadDeleteListModal').modal('hide');
        $('#fuelDeleteListModal').addClass('hide');
        $('#loadDeleteModal').modal('show');
        $('#btn-delete-load').addClass('hide');
        $('#btn-paid-fuel').addClass('hide');
        $('#btn-delete-fuel').removeClass('hide');
        $('.action-Desc-Upper').text('Delete');
        $('.action-Desc-lower').text('delete');
        $('.object-Desc-Upper').text('Purchase');
        $('.action-Desc-lower').text('purchase');
    });

    //Final Fuel Mark as Paid Oppourtunity
    $('.markaspaidthisFuelPurchase_btn').on('click', function (event) {
        var fID = $('input[name="ThisFuelPurchase_chk"]:checked').val();
        $('#delete-fuel-id').text(fID);
        $('#loadDeleteListModal').modal('hide');
        $('#fuelDeleteListModal').addClass('hide');
        $('#loadDeleteModal').modal('show');
        $('#btn-delete-load').addClass('hide');
        $('#btn-delete-fuel').addClass('hide');
        $('#btn-paid-fuel').removeClass('hide');
        $('.action-Desc-Upper').text('Mark');
        $('.action-Desc-lower').text('mark');
        $('.object-Desc-Upper').text('Purchase');
        $('.object-Desc-lower').text('purchase');
    });
    ////Add Load Button from Admin View Left - Launch Modal and Populate Drop Down Values
    $('.add-load-btn').on('click', function (event) {
        $('#Conf_Ind').val('1').removeClass('fileInput-maxx-validation');
        $('#BOL_Ind').val('1').removeClass('fileInput-maxx-validation');
        $("#openloadsAdd").load('../portal/GetAddLoad', function () { createTempLoadNbr() });
        $('#loadAddModal').modal({ backdrop: 'static', keyboard: false, show: true });
        $('.add-multi-dropoff-btn').removeClass('hide');
    });

    //Show more stops button
    $('#loadAddModal').on('click', '.add-multi-dropoff-btn', function (event) {
        $('.multi-dropoff').removeClass('hide');
        $('.add-multi-dropoff-btn').addClass('hide');
    });

    //Delete Load Button Click from Menu
    $('.delete-load-btn_driver').on('click', function (event) {
        resetAction();
        reloadIndex();
        $("#openloadsEditList").load('../portal/GetLoadChangeList?ChangeAction=delete');
        $('#loadDeleteListModal').modal('show');

    });

    //Delete Load Button Click from Menu
    $('.delete-load-btn').on('click', function (event) {
        resetAction();
        reloadIndex();
        $("#openloadsDeleteList").load('../portal/GetLoadChangeList?ChangeAction=delete');
        $('#loadDeleteListModal').modal('show');

    });

    //Status Update Button Click from Menu - Get List of Loads to update
    $('.update-status-btn').on('click', function (event) {
        $('#update-status-success').addClass('hide');
        $("#openloadsStatusList").load('../portal/GetLoadChangeList?ChangeAction=status');
        $('#loadStatusListModal').modal('show');
    });

    //Final Delete Oppourtunity
    $('.deletethisLoad_btn').on('click', function (event) {
        var load_num = $('input[name="editThisLoad_chk"]:checked').val();
        $('.delete-type-name').text('load');
        $('#delete-load-nbr').text(load_num);
        $('#loadDeleteListModal').modal('hide');
        $('#loadDeleteModal').modal('show');
        $('#btn-delete-load').removeClass('hide');
        $('#btn-delete-fuel').addClass('hide');
        $('#btn-paid-fuel').addClass('hide');
    });

    //Delete Load Method 
    $('#btn-delete-load').on('click', function (event) {
        var load_num = '';
        load_num = $('#delete-load-nbr').text();
        $('#loadDeleteModal').modal('hide');
        $('#loadDeleteListModal').modal('hide');

        $.ajax({
            method: "DELETE",
            url: '../portal/DeleteLoad?LoadNbr=' + load_num,
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);

                    resetAction();
                    reloadIndex();
                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);
                $('#Loading_Modal').modal('hide');
            }

        });
    });

    //Delete Fuel Method 
    $('#btn-delete-fuel').on('click', function (event) {
        var fuelID = '';
        fuelID = $('#delete-fuel-id').text();
        $('#loadDeleteModal').modal('hide');
        $('#loadDeleteListModal').modal('hide');

        $.ajax({
            method: "DELETE",
            url: '../portal/DeleteFuel?id=' + fuelID,
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);

                    resetAction();
                    reloadIndex();
                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);
                $('#Loading_Modal').modal('hide');
            }

        });
    });

    //Mark Fuel as Paid Method 
    $('#btn-paid-fuel').on('click', function (event) {
        var fuelID = '';
        fuelID = $('#delete-fuel-id').text();
        $('#loadDeleteModal').modal('hide');
        $('#loadDeleteListModal').modal('hide');

        $.ajax({
            method: "PUT",
            url: '../portal/MarkFuelPaid?id=' + fuelID,
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);

                    resetAction();
                    reloadIndex();
                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);
                $('#Loading_Modal').modal('hide');
            }

        });
    });



    //Save/Add Load Submit
    $('#btn-Add-Load').on('click', function (event) {
        $('#loading-title').text('Saving Changes...')
        var sourceForm = '';
        var disabled = null;
        var dataset = null;

        sourceForm = $('.loadaddform');
        disabled = sourceForm.find(':input:disabled').removeAttr('disabled');
        dataset = JSON.stringify($('.loadaddform').serializeAddModal());


        if ($('#Conf_Ind').val() == '0') {
            alert('Pickup Paperwork Must be uploaded before a load can be saved.')
            $('#addLoad-uploadbox').addClass('fileInput-maxx-validation');
        } else if ($('#BOL_Ind').val() == '0') {
            alert('Delivery Confirmation Paperwork Must be uploaded before a load can be saved.')
            $('#deliverLoad-uploadbox').addClass('fileInput-maxx-validation');
        }
        else {

            $.ajax({
                method: "POST",
                url: '../portal/AddLoad',
                dataType: "JSON",
                contentType: "application/json; charset=utf-8",
                data: dataset,
                success: function (response) {
                    if (response.success) {
                        console.log(response.responseText);
                        disabled.attr('disabled', 'disabled');
                        $('#loadAddModal').modal("hide");
                        $('#add-load-success').removeClass('hide');

                        resetAction();
                        reloadIndex();
                    } else {
                        alert(response.responseText);
                    }
                },
                error: function (response) {
                    alert(response.responseText);
                    $('#Loading_Modal').modal('hide');
                }

            });
        }
    });

    //Add disabled property on Edit Button on Close Edit Modal Click
    $('.editthisLoadClose_btn').on('click', function (event) {
        $('#edit-load-success').addClass('hide');
        $('#loadDeleteModal').modal('hide');
        $('#loadDeleteListModal').modal('hide');
        $('#fuelDeleteListModal').modal('hide');
        var editButton = document.getElementsByClassName("editthisLoad_btn");
        Array.prototype.forEach.call(editButton, function (el2) {
            el2.disabled = true;
        });

        var editButton = document.getElementsByClassName("deletethisLoad_btn");
        Array.prototype.forEach.call(editButton, function (el3) {
            el3.disabled = true;
        });
    });

    //Edit Modal Close Btn Modal to Modal 
    $('.btn-Edit-Modal-close').on('click', function (event) {
        $('#loadEditModal').modal('hide');
        $("#loadEditListModal").modal("hide");
    });

    // Save Edit Changes
    $('#btn-Update-Load').on('click', function (event) {
        // $('#Loading_Modal').modal('show');
        $('#loading-title').html('Saving Changes...')
        var sourceForm = null;
        var disabled = null;
        var load_num = null;
        var dataset = null;

        sourceForm = $('.loadeditform');
        disabled = sourceForm.find(':input:disabled').removeAttr('disabled');
        load_num = $('#vM_EditLoadData_Load_Number').val();
        dataset = JSON.stringify($('.loadeditform').serializeEditModal());

        $.ajax({
            method: "PUT",
            url: '../portal/EditLoad?LoadNbr=' + load_num,
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            data: dataset,
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);
                    disabled.attr('disabled', 'disabled');
                    $('#loadEditModal').modal('hide');
                    $('#edit-load-success').removeClass('hide');

                    resetAction();
                    reloadIndex();
                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);
                $('#Loading_Modal').modal('hide');
            }

        });

        $('#Loading_Modal').modal('hide');
    });

    // Save Load Status Changes
    $('.updatethisStatus_btn').on('click', function (event) {
        $('#loading-title').text('Saving Changes...');
        var load_num = '';
        var load_status = '';
        var load_payout = '';
        load_num = $('input[name="updateThisStatus_chk"]:checked').val();
        load_status = $('#LoadStatus_' + load_num + '').val();
        load_payout = $('input[name="updateThisStatus_chk"]:checked').attr('data-payout');

        if (load_status == 'Picked Up') {
            $('#loadStatusListModal').modal('hide');
            $('#loadPickupModal').modal('show');
            $('.pickupLoadNbr').text(load_num);
            $('.pickup-modal-title').html('Pickup Load - <span class="dtl_number">' + load_num + '</span>');
            $('#modal-pickup-text').text('This is a manual "Pick up" status update, would you like to bypass the Carrier Email Alert for this action.');
            $('#pickupEmailBypass').removeAttr('hidden');
        }
        if (load_status == 'Delivered') {
            $('#loadStatusListModal').modal('hide');
            $('#loadDeliveredModal').modal('show');
            $('.deliverLoadNbr').text(load_num);
            $('.delivered-modal-title').html('Deliver Load - <span class="dtl_number">' + load_num + '</span>');
            $('#modal-delivered-text-1').addClass('red').text('This is a manual "Delivered" status update, would you like to bypass the Carrier Email Alert for this action.');
            $('#deliverEmailBypass').removeAttr('hidden');
        }
        if (load_status == 'Paid') {
            $('#loadStatusListModal').modal('hide');
            $('#loadPaidModal').modal('show');
            $('.paidLoadNbr').text(load_num);
            $('.paid-modal-title').html('Paid Load  <span class="dtl_number">#' + load_num + ' - $' + load_payout + '</span>');
        }

    });


    //Reset Load Status - /*Admin Area Only*/ 
    $('#reset-status-btn').on('click', function (event) {
        var load_num = '';
        var resetType = '';
        load_num = $.trim($('#reset-Load-Number-TB').val());
        resetType = $(".admin-resetStatus-dd > .cs-options").find('li.cs-selected').attr('data-value');

        $.ajax({
            method: "GET",
            url: '../portal/resetLoad?pLoadNbr=' + load_num + '&pResetType=' + resetType + '',
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);

                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                $('#alert-load-status-reset').removeClass('hide');

                $('#load-reset-message').html('You have successfully reset load status');


            }

        });
    });

    //Save/Add Fuel Submit
    $('#btn-Add-Fuel').on('click', function (event) {
        $('#loading-title').text('Saving Changes...')
        var sourceForm = '';
        var disabled = null;
        var dataset = null;

        var driverNM = document.getElementById('vM_AddFuelData_Driver_Nbr').options[document.getElementById('vM_AddFuelData_Driver_Nbr').selectedIndex].innerHTML;
        document.getElementById('AddFuel_DriverNm').value = driverNM;

        $('input').removeClass('fileInput-maxx-validation');

        sourceForm = $('.fueladdform');
        disabled = sourceForm.find(':input:disabled').removeAttr('disabled');
        dataset = JSON.stringify($('.fueladdform').serializeAddFuelModal());


        if ($('#vM_AddFuelData_Fuel_Location').val() == '') {
            alert('A Fuel Location must be entered before a purchase can be saved.')
            $('#vM_AddFuelData_Fuel_Location').addClass('fileInput-maxx-validation');
        } else if ($('#vM_AddFuelData_Fuel_Gallons').val() == '') {
            alert('Gallons total must be entered before a purchase can be saved.')
            $('#vM_AddFuelData_Fuel_Gallons').addClass('fileInput-maxx-validation');
        }
        else if ($('#vM_AddFuelData_Fuel_Amount').val() == '') {
            alert('Purchase amount must be entered before a purchase can be saved.')
            $('#vM_AddFuelData_Fuel_Amount').addClass('fileInput-maxx-validation');
        }
        else if ($('#vM_AddFuelData_Odometer').val() == '') {
            alert('Odometer reading must be entered before a purchase can be saved.')
            $('#vM_AddFuelData_Odometer').addClass('fileInput-maxx-validation');
        }
        else if ($('#Receipt_Ind').val() == '0') {
            alert('A Fuel Receipt must be uploaded before a purchase can be saved.')
            $('#addFuel-uploadbox').addClass('fileInput-maxx-validation');
        }
        else {

            $.ajax({
                method: "POST",
                url: '../portal/AddFuelPurchase',
                dataType: "JSON",
                contentType: "application/json; charset=utf-8",
                data: dataset,
                success: function (response) {
                    if (response.success) {
                        console.log(response.responseText);
                        disabled.attr('disabled', 'disabled');
                        $('#fuelAddModal').modal("hide");
                        $('#add-fuel-success').removeClass('hide');

                        resetAction();
                        reloadIndex();
                    } else {
                        alert(response.responseText);
                    }
                },
                error: function (response) {
                    alert(response.responseText);
                    $('#Loading_Modal').modal('hide');
                }

            });
        }
    });

    // Serialize Add Load for DB
    $.fn.serializeAddModal = function () {
        var user = document.cookie;
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {

            if (this.name == "vM_AddLoadData.Pickup_Date") {
                if (this.value.length <= 5) {
                    var newValue = this.value + '/' + moment().year();
                    o["Pickup_Date"] = newValue || '';
                    console.log('Add Load - Pickup Date Fix Applied');
                } else {
                    o["Pickup_Date"] = this.value || '';
                }
            }
            o[this.name.replace("vM_AddLoadData.", "")] = this.value || '';

        });
        //Default Values
        o["Total_PPM"] = '0.00';
        o["Createdby"] = user;

        //console.log(0);

        return o;

    };

    // Serialize Edit Changes for DB
    $.fn.serializeEditModal = function () {
        var user = document.cookie;
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            //DB Column Exceptions
            if (this.name == "EditLoad_Weekof_Current") {
                o["Week_of"] = this.value || '';
            }
            if (this.name == "EditLoad_Customer_ID") {
                o["Customer_ID"] = this.value || '';
            }
            if (this.name == "vM_EditLoadData_Load_of_Week") {
                o["Load_of_Week"] = this.value || '';
            }
            if (this.name == "EditLoad_Driver_Nbr") {
                o["Driver_NBR"] = this.value || '';
            }
            if (this.name == "EditLoad_Truck_Nbr") {
                o["Truck_NBR"] = this.value || '';
            }
            if (this.name == "EditLoad_Trailer_Nbr") {
                o["Trailer_NBR"] = this.value || '';
            }
            if (this.name == "vM_EditLoadData.PPM") {
                o["Price_Per_Mile"] = this.value || '';
            }
            o[this.name.replace("vM_EditLoadData.", "")] = this.value || '';
        });
        //Default Values
        o["Updatedby"] = user;

        console.log(o);
        return o;
    };

    // Serialize Payroll Adjustment Modal Changes for DB
    $.fn.serializePayrollAdjustmentModal = function () {
        var user = document.cookie;  //Not Used
        var a = this.serializeArray();
        var new_array = [];
        var columnCount = 8; // number of columns in each row, only Variable that needs to be updated.
        var arrayOfArrays = [];
        var col_name;
        var col_val;

        col_name = a.map(function (a) { return a.name; });
        col_val = a.map(function (a) { return a.value; });

        for (var i = 0; i < col_name.length && i < col_val.length; i++) {
            new_array[i] = [col_name[i], col_val[i]];
        }

        for (var i = 0; i < new_array.length; i += columnCount) {  //creates new rows after 
            arrayOfArrays.push(new_array.slice(i, i + columnCount));
        }

        return arrayOfArrays;
    };

    // Serialize Expense Purpose Modal Changes for DB
    $.fn.serializeExpensePurposeModal = function () {
        var user = document.cookie;  //Not Used
        var a = this.serializeArray();
        var new_array = [];
        var columnCount = 8; // number of columns in each row, only Variable that needs to be updated.
        var arrayOfArrays = [];
        var col_name;
        var col_val;

        col_name = a.map(function (a) { return a.name; });
        col_val = a.map(function (a) { return a.value; });

        for (var i = 0; i < col_name.length && i < col_val.length; i++) {
            new_array[i] = [col_name[i], col_val[i]];
        }

        for (var i = 0; i < new_array.length; i += columnCount) {  //creates new rows after 
            arrayOfArrays.push(new_array.slice(i, i + columnCount));
        }

        return arrayOfArrays;
    };

    // Serialize Create Notefor DB
    $.fn.serializeCreateNote = function () {
        var user = document.cookie;
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (this.name == "vM_CreateNote_SentFrom") {
                o["SentFrom"] = this.value || '';
            }

            o[this.name.replace("vM_CreateNote.", "")] = this.value || '';

        });
        //Default Values
        o["Active_IND"] = '1';
        o["Message_Type_class"] = "SQL Side Populate";

        //console.log(o);


        return o;
    };

    // Serialize Add Load for DB
    $.fn.serializeAddFuelModal = function () {
        var user = document.cookie;
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            o[this.name.replace("vM_AddFuelData.", "")] = this.value || '';
        });
        console.log(o);

        return o;

    };

    // Mark load as Picked Up Button
    $('.loadPickup').on('click', function (event) {
        $('#loadPickupModal').modal('show');
        var loadnum = '';
        loadnum = $(this).attr("data-load");
        $('.pickupLoadNbr').text(loadnum);
    });

    // Mark load as Delivered Button
    $('.loadDelivered').on('click', function (event) {
        $('#loadDeliveredModal').modal('show');
        var loadnum = '';
        loadnum = $(this).attr("data-load");
        $('.deliverLoadNbr').text(loadnum);
    });

    // Pickup Load Button
    $('#btn-pickup-load').on('click', function (event) {
        var load_num = '';
        var load_status = '';
        load_num = $('.dLoadNbr').text();
        load_status = "Load Picked Up";
        var admin_ByPass = $('#pickupEmailBypass').val();
        var bypass_hidden = $('#pickupEmailBypass').is(':hidden')
        var bypass_email;
        if (bypass_hidden == true) {
            bypass_email = 'no';
        } else {
            bypass_email = admin_ByPass;
        }
        $.ajax({
            method: "PUT",
            url: '../portal/UpdateLoadStatus?LoadNbr=' + load_num + '&newStatus=' + load_status + '&bypassEmailNotification=' + bypass_email + '',
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);
                    $('#loadPickupModal').modal('hide');
                    $('#loadStatusListModal').modal('hide');
                    $('#pickup-load-success').removeClass('hide');


                    reloadIndex();
                    resetAction();
                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);
                $('#loadPickupModal').modal('hide');
            }

        });
    });

    // Deliver Load Button
    $('#btn-deliver-load').on('click', function (event) {
        var load_num = '';
        var load_status = '';
        var load_bol_url = '';
        var load_bol = '';
        load_num = $(this).attr("data-deliver");
        load_status = "Delivered";
        load_bol_url = $(this).attr("data-bol-url");
        load_bol = 'yes';
        var admin_ByPass = $('#deliverEmailBypass').val();
        var bypass_hidden = $('#deliverEmailBypass').is(':hidden')
        var bypass_email;
        if (bypass_hidden == true) {
            bypass_email = 'no';
        } else {
            bypass_email = admin_ByPass;
        }
        $.ajax({
            method: "PUT",
            url: '../portal/UpdateLoadStatus?LoadNbr=' + load_num + '&newStatus=' + load_status + '&BOLattached=' + load_bol + '&BOLurl=' + load_bol_url + '&bypassEmailNotification=' + bypass_email + '',
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);
                    $('#loadDeliveredModal').modal('hide');
                    $('#delivered-load-success').removeClass('hide');
                    $('#loadStatusListModal').modal('hide');
                    resetAction();
                    reloadIndex();
                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);
                $('#loadDeliveredModal').modal('hide');
            }

        });
    });

    // Paid Load Button
    $('#btn-paid-load').on('click', function (event) {
        var load_num = '';
        var load_status = '';
        var final_amount = '';
        var load_final_amount = '';
        var detention_amount = '';
        var detention_amount_final_amount = '';
        load_num = $('.pLoadNbr').text();
        load_status = "Paid";
        final_amount = $('#PaidLoad_Amount').val();
        load_final_amount = final_amount.replace(',', '').replace('$', '');
        detention_amount = $('#Dentention_Amount').val();
        detention_amount_final_amount = detention_amount.replace(',', '').replace('$', '');

        $.ajax({
            method: "PUT",
            url: '../portal/UpdateLoadStatus?LoadNbr=' + load_num + '&newStatus=' + load_status + '&loadPaidAmount=' + load_final_amount.trim() + '&detentionAmount=' + detention_amount_final_amount.trim() + '',
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);
                    $('#loadPaidModal').modal('hide');

                    resetAction();
                    reloadIndex();
                    $('#PaidLoad_Amount').val('');
                    $('#PaidLoad_Amount').html('');

                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);
                $('#Loading_Modal').modal('hide');
            }

        });
    });


    //Payroll - Run Payroll Button from page Click - Launch Modal 
    $('.payroll-run-btn').on('click', function (event) {
        $('#payroll_runPayrollModal').modal({ backdrop: 'static', keyboard: false, show: true });
    });


    //Payroll - Run Payroll - Execute Procedure
    $('.btn-run-payroll').on('click', function (event) {
        $.ajax({
            method: "GET",
            url: '../portal/RunPayroll',
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);
                    $('#payrollRunAlert').removeClass('hide');

                    delay(function () {
                        reloadPayrolltbl();
                    }, 2000);

                } else {
                    //   alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);
                $('#Loading_Modal').modal('hide');
            }

        });


    });

    //Payroll Adjustments Button from page Click - Launch Modal 
    $('.payroll-adj-btn').on('click', function (event) {
        $("#openpayrollAdjustment").load('../portal/GetPayrollAdjustmentDetail');
        $('#payroll_loadAdjModal').modal({ backdrop: 'static', keyboard: false, show: true });
    });

    // Save Payroll Adjustment Changes
    $('#btn-Update-Adj').on('click', function (event) {
        var sourceForm = $('.payrollAdjustmentForm');
        var disabled = sourceForm.find(':input:disabled').removeAttr('disabled');
        var dataset = JSON.stringify($('.payrollAdjustmentForm').serializePayrollAdjustmentModal());

        dataset = formatMultipleRowJsonForm(dataset);

        $.ajax({
            method: "PUT",
            url: '../portal/EditPayrollAdjustments',
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            data: dataset,
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);
                    disabled.attr('disabled', 'disabled');
                    $('#payroll_loadAdjModal').modal('hide');
                    $('#edit-load-success').removeClass('hide');
                    reloadPayrolltbl();
                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);
                $('#Loading_Modal').modal('hide');
            }

        });

    });

    //Closed Loads DateView Drop Down 
    $(".closedloads-dateview-dd > .cs-options").on('click', function (event) {
        var dd_node = this.querySelectorAll('li.cs-selected');
        var selected_val = dd_node[0].dataset.value;

        $('#closed-load-row').load('../portal/GetClosedLoadSummaryIndex?pViewDate=' + selected_val);

    });

    //Load Actions Drop Down 
    $(".load-action-dd > .cs-options").on('click', function (event) {
        var dd_node = this.querySelectorAll('li.cs-selected');
        var selected_val = dd_node[0].dataset.value;

        if (selected_val == "Add") {
            $("#openloadsAdd").load('../portal/GetAddLoad', function () { createTempLoadNbr() });
            $('#loadAddModal').modal({ backdrop: 'static', keyboard: false, show: true });
        }
        if (selected_val == "Edit") {
            $("#openloadsEditList").load('../portal/GetLoadChangeList?ChangeAction=edit');
            $('#loadEditListModal').modal('show');
        }
        if (selected_val == "Update") {
            $("#openloadsStatusList").load('../portal/GetLoadChangeList?ChangeAction=status');
            $('#loadStatusListModal').modal('show');
        }
        if (selected_val == "Delete") {
            $("#openloadsDeleteList").load('../portal/GetLoadChangeList?ChangeAction=delete');
            $('#loadDeleteListModal').modal('show');
        }

    });

    //Fuel Actions Drop Down 
    $(".fuel-action-dd > .cs-options").on('click', function (event) {
        var dd_node = this.querySelectorAll('li.cs-selected');
        var selected_val = dd_node[0].dataset.value;

        if (selected_val == "Add") {
            $('#Receipt_Ind').val('0').removeClass('fileInput-maxx-validation');
            $("#fuelPurchaseAdd").load('../portal/GetAddFuel', function () { null });
            $('#fuelAddModal').modal({ backdrop: 'static', keyboard: false, show: true });
        }
        if (selected_val == "Delete") {
            $('#fuelDeleteListModal').removeClass('hide');
            $("#openfuelDeleteList").load('../portal/GetFuelPurchasesList', function () { null });
            $('#fuelDeleteListModal').modal({ backdrop: 'static', keyboard: false, show: true });
            $('.fuel-edit-title-text').text('Delete');
            $('.markaspaidthisFuelPurchase_btn').addClass('hide');
            $('.deletethisFuelPurchase_btn').removeClass('hide');
        }
        if (selected_val == "Paid") { /*Delete Method being reused for Edit/Change Method*/
            $('#fuelDeleteListModal').removeClass('hide');
            $("#openfuelDeleteList").load('../portal/GetFuelPurchasesList', function () { null });
            $('#fuelDeleteListModal').modal({ backdrop: 'static', keyboard: false, show: true });
            $('.fuel-edit-title-text').text('Mark as Paid');
            $('.markaspaidthisFuelPurchase_btn').removeClass('hide');
            $('.deletethisFuelPurchase_btn').addClass('hide');
        }

    });


    //Payroll Summary Date Drop Down 
    $(".payroll-dateview-dd > .cs-options").on('click', function (event) {
        var dd_node = this.querySelectorAll('li.cs-selected');
        var selected_val = dd_node[0].dataset.value;

        $('#date-placeholder-tb').val(selected_val);

        var employeeInput = "";

        employeeInput = $('#employee-placeholder-tb').val();

        $("#payroll-row-data").load('../portal/GetPayrollSummaryIndex?pViewDate=' + selected_val + '&pEmployee=' + employeeInput);
    });

    //Payroll Summary Employee Drop Down 
    $(".payroll-personview-dd > .cs-options").on('click', function (event) {
        var dd_node = this.querySelectorAll('li.cs-selected');
        var selected_val = dd_node[0].dataset.value;

        $('#employee-placeholder-tb').val(selected_val);

        var dateInput = "";

        dateInput = $('#date-placeholder-tb').val();

        $("#payroll-row-data").load('../portal/GetPayrollSummaryIndex?pViewDate=' + dateInput + '&pEmployee=' + selected_val);
    });


    //Expenses Date Drop Down - Action
    $(".expenses-dateview-dd > .cs-options").on('click', function (event) {
        var dd_node = this.querySelectorAll('li.cs-selected');
        var selected_val = dd_node[0].dataset.value;

        $('#date-placeholder-tb').val(selected_val);

        var typeInput = "";

        typeInput = $('#type-placeholder-tb').val();

        $("#expenses-row-data").load('../portal/GetExpenseSummary?pViewDate=' + selected_val + '&pExpenseType=' + typeInput);
        $("#expenses-totals-header").load('../portal/GetExpenseTotalHeader?pViewDate=' + selected_val + '&pExpenseType=' + typeInput);


    });

    //Expenses Summary Authorzied by Drop Down 
    $(".expenses-typeview-dd > .cs-options").on('click', function (event) {


        var dd_node = this.querySelectorAll('li.cs-selected');
        var selected_val = dd_node[0].dataset.value;

        $('#type-placeholder-tb').val(selected_val);

        var dateInput = "";

        dateInput = $('#date-placeholder-tb').val();

        $("#expenses-row-data").load('../portal/GetExpenseSummary?pViewDate=' + dateInput + '&pExpenseType=' + selected_val);
        $("#expenses-totals-header").load('../portal/GetExpenseTotalHeader?pViewDate=' + dateInput + '&pExpenseType=' + selected_val);


    });

    //Fuel Date Drop Down - Action
    $(".fuel-dateview-dd > .cs-options").on('click', function (event) {
        var dd_node = this.querySelectorAll('li.cs-selected');
        var selected_val = dd_node[0].dataset.value;

        $('#fuel-date-placeholder-tb').val(selected_val);

        var employeeInput = "";

        employeeInput = $('#fuel-employee-placeholder-tb').val();

        $("#fuel-row-data").load('../portal/GetFuelSummary?pViewDate=' + selected_val + '&pEmployee=' + employeeInput);
        $("#fuel-totals-header").load('../portal/GetFuelTotalHeader?pViewDate=' + selected_val + '&pEmployee=' + employeeInput);
    });

    //Fuel Summary Authorzied by Drop Down 
    $(".fuel-personview-dd > .cs-options").on('click', function (event) {
        var dd_node = this.querySelectorAll('li.cs-selected');
        var selected_val = dd_node[0].dataset.value;

        $('#fuel-employee-placeholder-tb').val(selected_val);

        var dateInput = "";

        dateInput = $('#fuel-date-placeholder-tb').val();

        $("#fuel-row-data").load('../portal/GetFuelSummary?pViewDate=' + dateInput + '&pEmployee=' + selected_val);
        $("#fuel-totals-header").load('../portal/GetFuelTotalHeader?pViewDate=' + dateInput + '&pEmployee=' + selected_val);
    });

    //Financial Switch Month Drop Down - Action
    $(".financials-dateview-dd > .cs-options").on('click', function (event) {
        $('#financialLoading_Modal').modal("show");
        var dd_node = this.querySelectorAll('li.cs-selected');
        var selected_val = dd_node[0].dataset.value;

        $("#load-revenue-row-data").load('../portal/GetFinancialLoadGross?pViewDate=' + selected_val);
        $("#deposit-revenue-row-data").load('../portal/GetFinancialDeposits?pViewDate=' + selected_val);
        $("#fuel-revenue-row-data").load('../portal/GetFinancialFuel?pViewDate=' + selected_val);
        $("#payroll-revenue-row-data").load('../portal/GetFinancialPayroll?pViewDate=' + selected_val);
        $("#expenses-reoccurring-row-data").load('../portal/GetFinancialExpensesRO?pViewDate=' + selected_val);
        $("#expenses-revenue-row-data").load('../portal/GetFinancialExpenses?pViewDate=' + selected_val);

        financialTotalsUpdate(selected_val);

        delay(function () {
            $('#financialLoading_Modal').modal("hide");
        }, 2000);

    });

    //Weekly Production Table View vs. Driver/Truck
    $(".production-dataview-dd> .cs-options").on('click', function (event) {
        var dd_node = this.querySelectorAll('li.cs-selected');
        var selected_val = dd_node[0].dataset.value;

        $('#production-view-placeholder-tb').val(selected_val);

        if (selected_val == 'Weekly') {
            $('#production-detail-data').addClass('hide');
            $('#production-summary-data').removeClass('hide');
        }
        else {
            $('#production-summary-data').addClass('hide');
            $('#production-detail-data').removeClass('hide');
        }

    });

    //Weekly Production Table View Date View
    $(".production-dateview-dd > .cs-options").on('click', function (event) {
        var dd_node = this.querySelectorAll('li.cs-selected');
        var selected_val = dd_node[0].dataset.value;

        $('#production-date-placeholder-tb').val(selected_val);

        $("#production-detail-row-data").load('../portal/GetProductionDetailWeekly?pViewDate=' + selected_val);
        $("#production-summary-row-data").load('../portal/GetProductionWeekly?pViewDate=' + selected_val);
    });

    //Production Value Click Close 
    $('.production-value').on('click', function (event) {

        $('.production-value-active').removeClass('production-value-active');

        $('#production_Modal').css('padding-left', '0px');
        var datasource = '';
        var period = '';

        datasource = $(this).parent().attr('data-source');
        period = $(this).parent().attr('data-period');

        if (datasource == "gross") {
            $('#production-modal-title').html('Gross by Load - ' + period.replaceAll("2018-", "").replaceAll("2019-", "").replaceAll("2020-", ""));
            $('.mile-value').toggleClass('hide');
            $('.fuel-value').toggleClass('hide');
            $('.payroll-value').toggleClass('hide');
        } else if (datasource == "miles") {
            $('#production-modal-title').html('Miles by Day - ' + period.replaceAll("2018-", "").replaceAll("2019-", "").replaceAll("2020-", ""));
            $('.gross-value').toggleClass('hide');
            $('.fuel-value').toggleClass('hide');
            $('.payroll-value').toggleClass('hide');
        } else if (datasource == "fuel") {
            $('#production-modal-title').html('Fuel - ' + period.replaceAll("2018-", "").replaceAll("2019-", "").replaceAll("2020-", ""));
            $('.gross-value').toggleClass('hide');
            $('.mile-value').toggleClass('hide');
            $('.payroll-value').toggleClass('hide');
        } else if (datasource == "payroll") {
            $('#production-modal-title').html('Payroll - ' + period.replaceAll("2018-", "").replaceAll("2019-", "").replaceAll("2020-", ""));
            $('.gross-value').toggleClass('hide');
            $('.mile-value').toggleClass('hide');
            $('.fuel-value').toggleClass('hide');
        }

        $(this).addClass('production-value-active');

        $("#production-popover-row-data").load('../portal/GetProductionModal?pDataSource=' + datasource + '&pPeriod=' + period);

        $('#production_Modal').css({ 'padding-right': '0px' });

        var prodVal = store.get('productionModal');

    });

    //Production Value Click Close 
    $('.production-value-driver').on('click', function (event) {

        $('.production-value-active').removeClass('production-value-active');

        $('#production_Modal').css('padding-left', '0px');
        var datasource = '';
        var period = '';
        var entityRaw = '';
        var entity = '';

        datasource = $(this).parent().attr('data-source');
        period = $(this).parent().attr('data-period');
        entityRaw = $(this).parent().attr('data-entity');
        entity = entityRaw.replace('&nbsp', '%20');

        if (datasource == "gross") {
            $('#production-modal-title').html('Gross by Load - ' + period.replaceAll("2018-", "").replaceAll("2019-", "").replaceAll("2020-", ""));
            $('.mile-value').toggleClass('hide');
            $('.fuel-value').toggleClass('hide');
            $('.payroll-value').toggleClass('hide');
        } else if (datasource == "miles") {
            $('#production-modal-title').html('Miles by Day - ' + period.replaceAll("2018-", "").replaceAll("2019-", "").replaceAll("2020-", ""));
            $('.gross-value').toggleClass('hide');
            $('.fuel-value').toggleClass('hide');
            $('.payroll-value').toggleClass('hide');
        } else if (datasource == "fuel") {
            $('#production-modal-title').html('Fuel - ' + period.replaceAll("2018-", "").replaceAll("2019-", "").replaceAll("2020-", ""));
            $('.gross-value').toggleClass('hide');
            $('.mile-value').toggleClass('hide');
            $('.payroll-value').toggleClass('hide');
        } else if (datasource == "payroll") {
            $('#production-modal-title').html('Payroll - ' + period.replaceAll("2018-", "").replaceAll("2019-", "").replaceAll("2020-", ""));
            $('.gross-value').toggleClass('hide');
            $('.mile-value').toggleClass('hide');
            $('.fuel-value').toggleClass('hide');
        }

        $(this).addClass('production-value-active');

        $("#production-popover-row-data").load('../portal/GetProductionModal?pDataSource=' + datasource + '&pPeriod=' + period + '&pEntity=' + entity);

        $('#production_Modal').css({ 'padding-right': '0px' });

        var prodVal = store.get('productionModal');

    });



    //Production Modal Close
    $('.production-gross-close').on('click', function (event) {
        $('#production_Modal').modal('hide');

    });

    function financialTotalsUpdate(newdate) {

        $.ajax({
            method: "GET",
            url: '../portal/reloadFinancials?pViewDate=' + newdate,
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);

                } else {
                    // alert(response.responseText);
                }
            },
            error: function (response) {  //portaling Error 

                $("#NET-PNL-Value").load('../portal/reloadFinancials_NET');
                $("#Load-Revenue-Value").load('../portal/reloadFinancials_Revenue');
                $("#Fuel-Total-Value").load('../portal/reloadFinancials_Fuel');
                $("#Payroll-Total-Value").load('../portal/reloadFinancials_Payroll');
                $("#Expenses-Total-Value").load('../portal/reloadFinancials_Expenses');
                $('#financialLoading_Modal').modal("hide");

                //var netval = $('#NET-PNL-Value').text();
                //var trueval = netval.replace("-", "").replace("+", "").replace(",", "").replace("$", "");
                //var numberval = parseInt(trueval);
                //if (numberval <= 0) {

                //    console.log(numberval);
                //    $('#NET-PNL-Value').removeClass(".net-value-green").addClass("net-value-red");
                //} 
                //if (numberval > 0) {

                //    console.log(numberval);

                //    $('#NET-PNL-Value').removeClass(".net-value-red").addClass("net-value-green");
                //}

            }
        });

    };


    //Get Expense - for purpose edit
    $(document).on("click", ".add-expense-purpose", function (e) {
        var expense_ID = $(this).attr('data-expenseid');
        $("#expense-purpose-row").load('../portal/GetExpenseDetail?exID=' + expense_ID);
        $('#expense_purpose_Modal').modal({ backdrop: 'static', keyboard: false, show: true });
    });

    //Edit Expense - save changes
    $('.update-expense-purpose').on('click', function (event) {
        var sourceForm = $('.expensePurposeForm');
        var dataset = JSON.stringify($('.expensePurposeForm').serializeExpensePurposeModal());
        var dateInput = "";
        var employeeInput = "";

        dataset = formatMultipleRowJsonForm(dataset);
        dateInput = $('#date-placeholder-tb').val();
        employeeInput = $('#employee-placeholder-tb').val();

        $.ajax({
            method: "PUT",
            url: '../portal/EditExpensePurpose',
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            data: dataset,
            success: function (response) {
                if (response.success) {
                    console.log(response.responseText);
                    $('.expense-update-success').removeClass('hide');
                    $('#expense_purpose_Modal').modal('hide');
                    $("#expenses-row-data").load('../portal/GetExpenseSummary?pViewDate=' + dateInput + '&pEmployee=' + employeeInput);
                    $("#expenses-revenue-row-data").load('../portal/GetFinancialExpenses');
                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);
                $('#Loading_Modal').modal('hide');
            }

        });

    });


    //Financials Panels
    $(document).on('click', '.panel-heading span.clickable', function (e) {
        var $this = $(this);
        if (!$this.hasClass('panel-collapsed')) {
            $this.parents('.panel').find('.panel-body').slideUp();
            $this.addClass('panel-collapsed');
            $this.find('i').removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');
        } else {
            $this.parents('.panel').find('.panel-body').slideDown();
            $this.removeClass('panel-collapsed');
            $this.find('i').removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');
        }
    })

    //Setup Default info for Sand Loads
    function createTempLoadNbr() {

        var vDate = new Date();
        var vDateString;

        vDate.setDate(vDate.getDate());
        vDateString = ('0' + (vDate.getMonth() + 1)).slice(-2) + '-'
            + ('0' + (vDate.getDate())).slice(-2) + '-'
            + vDate.getFullYear();


        $.get("../portal/getMaxDayLoadCount?date_Val=" + vDateString, function (data) {

            var date = new Date();
            var dateString = new Date(date.getTime() - (date.getTimezoneOffset() * 60000))
                .toISOString()
                .split("T")[0];

            var load_id = dateString.replace(/-/g, '') + '-' + data[0];
            $('#vM_AddLoadData_Load_Number').val(load_id);
        });

        $("#vM_AddLoadData_Route_ID").find("option").addClass("select-route");
    }


    //Reload All non parameterized Actions
    function reloadIndex() {
        // $('#index-company-stats').load('../portal/GetIndexStats');
        $('#open-load-row').load('../portal/GetOpenLoadSummaryIndex');
        $('#closed-load-row').load('../portal/GetClosedLoadSummaryIndex');
        $("#openloadsEditList").load('../portal/GetLoadChangeList?ChangeAction=edit');
        $("#fuel-purchase-row").load('../portal/GetFuelPurchases');
        $("#openloadsStatusList").load('../portal/GetLoadChangeList?ChangeAction=status');

        $('#loadDeleteModal').modal('hide');
        $('#fuelDeleteListModal').modal('hide');
        $('#Loading_Modal').modal('hide');
        $('.modal-backdrop').hide();
    }

    function reloadNotesAlerts() {
        $('#web-notes-rows').load('../portal/WebNotesIndex');
        $('#web-alerts-rows').load('../portal/WebAlertsIndex');
        $("#note-count-notification").load('../portal/WebNotesCountIndex');
    }

    function resetAction() {
        $('.cs-options').find('li').removeClass('cs-selected'); //start here
        $('.cs-placeholder').html('Choose Action');
    }

    function resetAdmin() {
        $(".admin-resetStatus-dd").find('.cs-options').find('li').removeClass('cs-selected');
        $(".admin-resetStatus-dd > .cs-options").find('.cs-placeholder').html('Choose Reset Status ');
    }


    function reloadPayrolltbl() {
        $("#payroll-row-data").load('../portal/GetPayrollSummaryIndex');
    }

    function formatMultipleRowJsonForm(obj) {
        var newobj = obj.replace(new RegExp("\"\,\"", "g"), '":"').replace(new RegExp(/\[\[\"/, 'g'), "{\"").replace(new RegExp(/\"\]\]/, 'g'), "\"\}").replace(new RegExp(/\]\,\[/, 'g'), ",");

        return newobj;
    }

    String.prototype.replaceAll = function (str1, str2, ignore) {
        return this.replace(new RegExp(str1.replace(/([\/\,\!\\\^\$\{\}\[\]\(\)\.\*\+\?\|\<\>\-\&])/g, "\\$&"), (ignore ? "gi" : "g")), (typeof (str2) == "string") ? str2.replace(/\$/g, "$$$$") : str2);
    }

    //Session Expired Logout Modal Btn
    $('#btn-LogoutNow').on('click', function (event) {
        forceLogout();
    });

    //Start Session Timeout Calucations //

    var sessServerAliveTime = 10 * 60 * 2;
    var sessionTimeout = 1 * 60000;
    var sessLastActivity;
    var idleTimer, remainingTimer;
    var isTimout = false;

    var sess_intervalID, idleIntervalID;
    var sess_lastActivity;
    var timer;
    var isIdleTimerOn = false;
    localStorage.setItem('sessionSlide', 'isStarted');

    $(window).scroll(function (e) {
        localStorage.setItem('sessionSlide', 'isStarted');
        startIdleTime();
    });

    $(window).mousemove(function (e) {
        localStorage.setItem('sessionSlide', 'isStarted');
        startIdleTime();
    });

    $(window).mousedown(function (e) {
        localStorage.setItem('sessionSlide', 'isStarted');
        startIdleTime();
    });

    function startIdleTime() {
        stopIdleTime();
        localStorage.setItem("sessIdleTimeCounter", $.now());
        idleIntervalID = setInterval(checkIdleTimeout(), 1000);
        global_last_idle_time = checkIdleTimeout();
        isIdleTimerOn = false;
    }

    function stopIdleTime() {
        clearInterval(idleIntervalID);
        clearInterval(remainingTimer);
        global_idle_counter = 0;
    }

    function checkIdleTimeout() {
        var idleTime = (parseInt(localStorage.getItem('sessIdleTimeCounter')) + (sessionTimeout));
        global_idle_counter = ((idleTime - global_last_idle_time) / 1000);

        return idleTime;
    }

    //End Session Timeout Calucations //

});

