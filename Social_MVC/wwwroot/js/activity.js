function confirmDelete(url)
{
    Swal.fire({
        title: "Är du säker?",
        text: "Du kan inte få tillbaka aktiviteten",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Ja, ta bort!",
        cancelButtonText: "Avbryt"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function(response) {
                    Swal.fire({
                        title: "Deleted!",
                        text: "Your activity has been deleted.",
                        icon: "success"
                    }).then(() => {
                        location.reload();
                    });
                },
                error: function(xhr, status, error) {
                    Swal.fire({
                        title: "Error!",
                        text: "There was an error deleting the activity.",
                        icon: "error"
                    });
                }
            });
        }
    });
}

function confirmLeave(url) {
    Swal.fire({
        title: "Är du säker?",
        text: "Du kommer att lämna eventet",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Ja, lämna!",
        cancelButtonText: "Avbryt"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'POST',
                success: function(response) {
                    Swal.fire({
                        title: "Lämnat!",
                        text: "Du har lämnat eventet.",
                        icon: "success"
                    }).then(() => {
                        location.reload();
                    });
                },
                error: function(xhr, status, error) {
                    Swal.fire({
                        title: "Fel!",
                        text: "Det gick inte att lämna eventet.",
                        icon: "error"
                    });
                }
            });
        }
    });
}