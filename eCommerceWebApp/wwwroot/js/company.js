// Variable to store the data for the table
var dataTable;

// Call the load table function when page is loaded
$(document).ready(function () {
    loadDataTable();
});

// Function to populate the table data and config the DataTables.net script
function loadDataTable() {
    dataTable = $('#companyTable').DataTable({

        "ajax": {
            "url":"/Admin/Company/GetAllCompanies"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "address", "width": "20%" },
            { "data": "city", "width": "10%" },
            { "data": "postCode", "width": "10%" },
            { "data": "phoneNumber", "width": "10%" },

            //Add buttons to the last column
            {
                "data": "id", // data gets the Id
                "render": function (data) { // data = Id
                    // Return the HTML code for the buttons rendering and Id -- No ASP TAG Helpers inside javaScript
                    return ` 
                        <div class="btn-group w-75" role="group">
                            <a href="/Admin/Company/UpsertCompany?id=${data}"
                                class="btn btn-info btn-sm "> <i class="bi bi-pencil-square"></i>&nbsp;Edit
                            </a>
                            <a href="/Admin/Company/DetailsCompany?id=${data}"
                                class="btn btn-primary btn-sm "><i class="bi bi-zoom-in"></i>&nbsp;Details
                            </a>
                            <a class="btn btn-danger btn-sm " onClick="DeleteCompany('/Admin/Company/DeleteCompany/${data}')">
                                <i class="bi bi-trash"></i>&nbsp;Delete
                            </a>
                        </div>
                    `
                },
                "width": "25%"
            }
        ]
    });
}

// Function to delete and display alerts
function DeleteCompany(url) {
    Swal.fire({ //Alert
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => { //Result
        if (result.isConfirmed) {
            $.ajax({ //Call for the API Delete
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload(); //Reload table data
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
            Swal.fire(
                'Deleted!',
                'Your file has been deleted.',
                'success'
            )
        }
    })
}