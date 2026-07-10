let currentPage = 1;
let pageSize = 10;
let deleteId = 0;

$(document).ready(function () {

    loadRoles();
    loadUsers();

    //search code
    let searchTimer;

    $("#txtSearchName").on("keyup", function () {

        clearTimeout(searchTimer);

        searchTimer = setTimeout(function () {

            const searchText = $("#txtSearchName").val().trim();

            if (searchText.length >= 2 || searchText.length === 0) {

                currentPage = 1;
                loadUsers();
            }

        }, 300);

    });
    //
    $("#btnClear").on("click", function () {
        $("#txtSearchName").val("");
        currentPage = 1;
        loadUsers();
    });

    $("#btnAddUser").on("click", function () {
        clearModal();
        $("#modalTitle").text("Add User");
        $("#passwordDiv").show();
        $("#userModal").modal("show");
    });

    $("#btnSaveUser").on("click", function () {
        saveUser();
    });

    $("#btnDeleteConfirm").on("click", function () {
        deleteUser();
    });

    $("#btnTogglePassword").on("click", function () {

        const txtPassword = $("#txtPassword");

        if (txtPassword.attr("type") === "password") {

            txtPassword.attr("type", "text");
            $(this).text("Hide");
        }
        else {

            txtPassword.attr("type", "password");
            $(this).text("Show");
        }

    });
});

function loadUsers(page) {

    if (page)
        currentPage = page;

    $.ajax({
        url: webRoot + "api/login/get-users",
        type: "GET",
        data: {
            pageNumber: currentPage,
            pageSize: pageSize,
            name: $("#txtSearchName").val()
        },
        success: function (response) {

            bindTable(response.Items);
            buildPager(response.TotalCount);
        },
        error: function (xhr) {
            console.log(xhr);
            showMessage("Error loading users.");
        }
    });

}

function bindTable(users) {

    const tbody = $("#tblUsers tbody");
    tbody.empty();


    if (!users || users.length === 0) {

        tbody.append(`
        <tr>
            <td colspan="6" class="text-center">
                No records found.
            </td>
        </tr>
    `);

        return;
    }

    users.forEach((x, index) => {

        let srNo = ((currentPage - 1) * pageSize) + index + 1;

        const role =
            x.RoleId === 1
                ? "Admin"
                : "User";

        const status = x.IsActive
            ? '<span class="badge bg-success">Active</span>'
            : '<span class="badge bg-danger">Inactive</span>';

        tbody.append(`
        <tr>
            <td>${srNo}</td>
            <td>${x.FullName}</td>
            <td>${x.Email}</td>
            <td>${role}</td>
            <td>${status}</td>
            <td>
                <button
                    class="btn btn-sm btn-primary btn-edit"
                    data-id="${x.LoginId}">
                    Edit
                </button>
            </td>
        </tr>
    `);
    });

    $(".btn-edit").off("click").on("click", function () {
        editUser($(this).data("id"));
    });

    $(".btn-delete").off("click").on("click", function () {
        deleteId = $(this).data("id");
        $("#deleteModal").modal("show");
    });

}

function buildPager(totalCount) {

    const pager = $("#pager");
    pager.empty();

    const totalPages =
        Math.ceil(totalCount / pageSize);

    if (totalPages <= 1)
        return;

    for (let i = 1; i <= totalPages; i++) {

        const active =
            currentPage === i
                ? "active"
                : "";

        pager.append(`
        <li class="page-item ${active}">
            <a href="#"
               class="page-link page-number"
               data-page="${i}">
                ${i}
            </a>
        </li>
    `);
    }

    $(".page-number").off("click").on("click", function (e) {
        e.preventDefault();

        const page =
            $(this).data("page");

        loadUsers(page);
    });

}

function loadRoles() {

    $.ajax({
        url: webRoot + "api/login/get-roles",
        type: "GET",
        success: function (roles) {

            const ddl = $("#ddlRole");

            ddl.empty();

            roles.forEach(x => {

                ddl.append(`
                <option value="${x.RoleId}">
                    ${x.Role}
                </option>
            `);
            });
        },
        error: function () {
            showMessage("Error loading roles.");
        }
    });

}

function saveUser() {

    const id = $("#hdnLoginId").val();

    const user = {
        LoginId: id,
        FullName: $("#txtFullName").val().trim(),
        Email: $("#txtEmail").val().trim(),
        Password: $("#txtPassword").val(),
        RoleId: parseInt($("#ddlRole").val()),
        IsActive: $("#chkActive").is(":checked")
    };

    const passwordRegex =
        /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@@$!%*?&])[A-Za-z\d@@$!%*?&]{8,}$/;
    const nameRegex = /^[A-Za-z]+(?: [A-Za-z]+)*$/;

    const emailRegex =
        /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/;

    // Full Name
    if (!user.FullName) {

        showMessage("Full Name is required.");
        return;
    }

    if (!nameRegex.test(user.FullName)) {

        showMessage("Full Name should contain only alphabets and spaces.");
        return;
    }

    // Email
    if (!user.Email) {

        showMessage("Email is required.");
        return;
    }

    if (!emailRegex.test(user.Email)) {

        showMessage("Please enter a valid email address.");
        return;
    }

    // Role
    if (!user.RoleId || isNaN(user.RoleId)) {

        showMessage("Please select a role.");
        return;
    }

    if (id === "") {

        if (!user.Password) {

            showMessage("Password is required.");

            return;
        }

        if (!passwordRegex.test(user.Password)) {

            showMessage(
                "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number and one special character."
            );

            return;
        }

        $.ajax({
            url: webRoot + "api/login/create-user",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(user),
            success: function () {

                $("#userModal").modal("hide");
                loadUsers();

                showMessage("User created successfully.");
            },
            error: function (xhr) {
                console.log(xhr);
                showMessage("Error creating user.");
            }
        });
    }
    else {

        delete user.Password;

        $.ajax({
            url:
                webRoot +
                "api/login/update-user/" +
                id,
            type: "PUT",
            contentType: "application/json",
            data: JSON.stringify(user),
            success: function () {

                $("#userModal").modal("hide");
                loadUsers();

                showMessage("User updated successfully.");
            },
            error: function (xhr) {
                console.log(xhr);
                showMessage("Error updating user.");
            }
        });
    }

}

function editUser(id) {

    $.ajax({
        url:
            webRoot +
            "api/login/get-user/" +
            id,
        type: "GET",
        success: function (user) {

            clearModal();

            $("#modalTitle").text("Edit User");

            $("#passwordDiv").hide();

            $("#hdnLoginId").val(user.LoginId);
            $("#txtFullName").val(user.FullName);
            $("#txtEmail").val(user.Email);
            $("#ddlRole").val(user.RoleId);
            $("#chkActive").prop(
                "checked",
                user.IsActive);

            $("#userModal").modal("show");
        },
        error: function () {
            showMessage("Error loading user.");
        }
    });

}

function deleteUser() {

    $.ajax({
        url:
            webRoot +
            "api/login/delete-user/" +
            deleteId,
        type: "DELETE",
        success: function () {

            $("#deleteModal").modal("hide");

            loadUsers();

            showMessage("User deleted successfully.");
        },
        error: function () {
            showMessage("Error deleting user.");
        }
    });

}

function clearModal() {

    $("#hdnLoginId").val("");
    $("#txtFullName").val("");
    $("#txtEmail").val("");
    $("#txtPassword").val("");
    $("#ddlRole").val("");
    $("#chkActive").prop("checked", true);
    $("#txtPassword").attr("type", "password");
    $("#btnTogglePassword").text("Show");

}

function showMessage(message, title = "Message") {

    $("#messageTitle").text(title);
    $("#messageBody").text(message);

    $("#messageModal").modal("show");
}