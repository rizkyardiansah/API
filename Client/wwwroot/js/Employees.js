$(document).ready(function () {
    $("#employees-table").DataTable({
        dom: "Bftrip",
        buttons: [
            'excel',
            {
                extend: 'pdf',
                orientation: 'landscape',
                pageSize: 'LEGAL'
            }
        ],
        "ajax": {
            "url": "https://localhost:44314/api/employees/masteremployees/",
            "dataSrc": ""
        },
        "columns": [
            { "data": "nik" },
            { "data": "fullName" },
            //{ "data": "gender" },
            {
                "data": "birthDate",
                "render": function (data, type, row) {
                    return data.split("T")[0]
                }
            },
            { "data": "email" },
            { "data": "phone" },
            { "data": "universityName" },
            { "data": "degree" },
            //{ "data": "gpa" },
            {
                "data": "salary",
                "render": function (data, type, row) {
                    return `Rp ${data}`
                }
            },
            {
                "data": null,
                "render": function (data, type, row) {
                    return `<button class="btn btn-warning" data-toggle="modal" onclick="showEditModal(${row['nik']})" data-target="#employeeEditModal"><i class="fa fa-list" aria-hidden="true"></i></button>
                        <button class="btn btn-danger" onclick="removeEmployee(${row['nik']})"><i class="fa fa-minus-circle" aria-hidden="true"></i></button></td>`
                }
            }
        ],
    });

    $.ajax({
        url: "https://localhost:44314/api/universities"
    }).done(result => {
        let options = "";
        for (let univ of result) {
            options += `<option value="${univ.id}">${univ.name}</option>`
        }
        $("#formEmployee #universityId").html(options);
    }).fail(error => console.log(error))

    $("#formEmployee").validate({
        submitHandler: function (form) {
            postEmployee()
        },
        errorElement: "small",
        errorClass: "text-danger",
        errorPlacement: function (error, element) {
            if (element.attr("name") == "gender") {
                error.insertAfter(element.parent().parent())
            } else {
                error.insertAfter(element)
            }
        }
    });


    $("#formEditEmployee").validate({
        submitHandler: function (form) {
            putEmployee()
        },
        errorElement: "small",
        errorClass: "text-danger",
        errorPlacement: function (error, element) {
            if (element.attr("name") == "gender") {
                error.insertAfter(element.parent().parent())
            } else {
                error.insertAfter(element)
            }
        }
    });

    const genderChartOptions = {
        chart: {
            type: "donut",
        },
        series: [28, 30],
        labels: ['Male', 'Female'],
        dataLabels: {
            enabled: false,
        },
        plotOptions: {
            pie: {
                donut: {
                    size: "50%",
                    labels: {
                        show: true,
                        name: {
                            show: true,

                        },
                        value: {
                            show: true,
                        },
                        total: {
                            show: true,
                            color: '#333',
                        }
                    }
                }
            }
        }
    }

    const universityChartOptions = {
        chart: {
            type: "bar",
        },
        theme: {
            palette: 'palette1',
        },
        series: [{
            data: [
                {
                    x: "Oxford",
                    y: 10,
                }, {
                    x: "Waterloo",
                    y: 4,
                }, {
                    x: "Stanford",
                    y: 11,
                }, {
                    x: "UCLA",
                    y: 7,
                }, {
                    x: "MIT",
                    y: 3,
                }, {
                    x: "Harvard",
                    y: 2,
                }, {
                    x: "Rice",
                    y: 5,
                }
            ]
        }],
        plotOptions: {
            bar: {
                vertical: true,
            }
        },
    }

    const genderChart = new ApexCharts(document.getElementById("genderChart"), genderChartOptions)
    const universityChart = new ApexCharts(document.getElementById("universityChart"), universityChartOptions)

    genderChart.render();
    universityChart.render();
})


function postEmployee() {

    let obj = new Object()

    obj.firstName = $("#formEmployee #firstName").val();
    obj.lastName = $("#formEmployee #lastName").val();
    obj.phone = $("#formEmployee #phone").val();
    obj.birthDate = $("#formEmployee #birthDate").val();
    obj.salary = parseInt($("#formEmployee #salary").val());
    obj.gender = $("#formEmployee #gender input[name=gender]:checked").val();
    obj.email = $("#formEmployee #email").val();
    obj.password = "asdf";
    obj.degree = $("#formEmployee #degree").val();
    obj.gpa = $("#formEmployee #gpa").val();
    obj.universityId = parseInt($("#formEmployee #universityId option:selected").val());

    console.log(obj)

    $.ajax({
        url: "https://localhost:44314/api/accounts/register",
        type: "POST",
        data: JSON.stringify( obj ),
        contentType: "application/json",
        dataType: "json"
    }).done(result => {
        console.log(result)
    }).fail(error => {
        console.log(error)
        if (error.status == 200) {
            Swal.fire(
                'Add Employee',
                'Employee successfully added.',
                'success'
            ).then(result => {
                if (result.isConfirmed) {
                    window.location.replace("/employees/index");
                }
            })
        } else {
            Swal.fire(
                'Add Employee',
                'Fail to add employee.',
                'error'
            )
        }
    })
}

function showEditModal(nik) {
    $.ajax({
        url: `https://localhost:44314/api/employees/${nik}`
    }).done(result => {
        console.log(result)
        $("#employeeEditModal #employeeEditModalTitle").text(result.nik)
        $("#employeeEditModal #formEditEmployee #nik").val(result.nik)
        $("#employeeEditModal #formEditEmployee #firstName").val(result.firstName)
        $("#employeeEditModal #formEditEmployee #lastName").val(result.lastName)
        $("#employeeEditModal #formEditEmployee #phone").val(result.phone)
        $("#employeeEditModal #formEditEmployee #salary").val(result.salary)
        $("#employeeEditModal #formEditEmployee #email").val(result.email)

        const birthDate = new Date(result.birthDate)
        const day = birthDate.getDate().toString().padStart(2, "0")
        const month = (birthDate.getMonth() + 1 ).toString().padStart(2, "0")
        const birthDateString = `${birthDate.getFullYear()}-${month}-${day}`;
        console.log(birthDateString)
        $("#formEditEmployee #birthDate").val(birthDateString);
        
        if (result.gender == 0) {
            $("#formEditEmployee #gender input[value=Male]").attr("checked", true);
            $("#formEditEmployee #gender input[value=Female]").attr("checked", false);
        } else {
            $("#formEditEmployee #gender input[value=Female]").attr("checked", true);
            $("#formEditEmployee #gender input[value=Male]").attr("checked", false);
        }

        $("#employeeEditModal .modal-footer button[type=submit]").on('click', function () {
            
        })
    }).fail(error => {
        console.log(error)
    })
}

function putEmployee() {
    const newEmployeeData = new Object()

    const nik = $("#formEditEmployee #nik").val();
    newEmployeeData.nik = nik;
    newEmployeeData.firstName = $("#formEditEmployee #firstName").val();
    newEmployeeData.lastName = $("#formEditEmployee #lastName").val();
    newEmployeeData.phone = $("#formEditEmployee #phone").val();
    newEmployeeData.birthDate = $("#formEditEmployee #birthDate").val();
    newEmployeeData.salary = parseInt($("#formEditEmployee #salary").val());
    newEmployeeData.gender = $("#formEditEmployee #gender input[name=gender]:checked").val();
    newEmployeeData.email = $("#formEditEmployee #email").val();

    console.log(newEmployeeData)

    $.ajax({
        url: `https://localhost:44314/api/employees/${nik}`,
        type: "PUT",
        data: JSON.stringify(newEmployeeData),
        contentType: "application/json",
        dataType: "json"
    }).done(result => {
        Swal.fire(
            'Edit Employee',
            'Employee successfully edited.',
            'success'
        ).then(result => {
            if (result.isConfirmed) {
                window.location.replace("/employees/index");
            }
        })
       
    }).fail(error => {
        Swal.fire(
            'Edit Employee',
            'Fail to edit employee.',
            'error'
        )
    })
}

function removeEmployee(nik) {
    console.log(nik)

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `https://localhost:44314/api/employees/${nik}`,
                type: "DELETE"
            }).done(result => {
                Swal.fire(
                    'Deleted!',
                    'An Employee has been deleted.',
                    'success'
                ).then(result => {
                    if (result.isConfirmed) {
                        window.location.replace("/employees/index");
                    }
                })
                //window.location.replace("/employees/index");
            }).fail(error => {
                Swal.fire(
                    'Failed!',
                    error.message,
                    'error'
                )
            })

        } else if (
            /* Read more about handling dismissals below */
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire(
                'Cancelled',
                'Employee data is safe',
                'error'
            )
        }
    })


}
