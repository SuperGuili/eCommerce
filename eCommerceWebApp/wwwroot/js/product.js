// Variable to store the data for the table
var dataTable;

// Call the load table function when page is loaded
$(document).ready(function () {
    loadDataTable();
});

// Function to populate the table data and config the DataTables.net script
function loadDataTable() {
    dataTable = $('#productTable').DataTable({

        "ajax": {
            "url":"/Admin/Product/GetAllProducts"
        },
        "columns": [
            { "data": "productName", "width": "35%" },
            { "data": "stockQuantity", "width": "5%" },
            { "data": "price", "width": "10%" },
            { "data": "category.categoryName", "width": "15%" },
            { "data": "tag.tagName", "width": "10%" },
            //Add buttons to the last column
            {
                "data": "id", // data gets the Id
                "render": function (data) { // data = Id
                    // Return the HTML code for the buttons rendering and Id -- No ASP TAG Helpers inside javaScript
                    return ` 
                        <div class="btn-group w-75" role="group">
                            <a href="/Admin/Product/UpsertProduct?id=${data}"
                                class="btn btn-info btn-sm mx-2"> <i class="bi bi-pencil-square"></i>&nbsp;Edit
                            </a>
                            <a class="btn btn-danger btn-sm mx-2" onClick="DeleteProduct('/Admin/Product/DeleteProduct/${data}')">
                                <i class="bi bi-trash"></i>&nbsp;Delete
                            </a>
                        </div>
                    `
                },
                "width": "30%"
            }
        ]
    });
}

// Function to delete and display alerts
function DeleteProduct(url) {
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