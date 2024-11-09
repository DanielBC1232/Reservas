//mostrar - ocultar contraseña
$(document).ready(function () {
    //  cambiar tipo de input (text- password) input confirmar contraseña

    //establece por defecto el icono de oculto
    $("#showPass1").removeClass("fa fa-eye").addClass("fa fa-eye-slash");

    //funcion que cambia el mostrar/ocultar al dar click
    $("#showPass1").click(function () {
        //captura el html
        const passwordInput1 = $("#inputPass");
        //del html capturado, captura el tipo de input que es en el presente
        const type1 = passwordInput1.attr("type");

        //si es tipo password cambiar a tipo texto junto al tipo de icono del ojo
        if (type1 === "password") {
            passwordInput1.attr("type", "text");
            $("#showPass1").removeClass("fa fa-eye-slash").addClass("fa fa-eye");
        } else {
            //si no hacer lo contrario
            passwordInput1.attr("type", "password");
            $("#showPass1").removeClass("fa fa-eye").addClass("fa fa-eye-slash");
        }
    });
});

$(document).ready(function () {

    //  cambiar tipo de input (text- password) input confirmar contraseña
    $("#showPass2").removeClass("fa fa-eye").addClass("fa fa-eye-slash");
    $("#showPass2").click(function () {
        const passwordInput2 = $("#inputPassConfirm");
        const type2 = passwordInput2.attr("type");

        if (type2 === "password") {
            passwordInput2.attr("type", "text");
            $("#showPass2").removeClass("fa fa-eye-slash").addClass("fa fa-eye");
        } else {
            passwordInput2.attr("type", "password");
            $("#showPass2").removeClass("fa fa-eye").addClass("fa fa-eye-slash");
        }
    });
});

//al presionar boton crearusuario
$(document).on('click', '#btnCrearUsuario', function () {

    //tomar valores de los campos y guardarlos en variables
    var usuario = document.getElementById('inputUsuario').value.trim();

    var correo = document.getElementById('inputCorreo').value.trim();

    var pass = document.getElementById('inputPass').value.trim();

    var passConfirm = document.getElementById('inputPassConfirm').value.trim();

    //.trim remueve espacios en blanco derecha e izquierda
    //comprobar si los datos de los campos son validos
    //variable que al final define si se hace el registro o no
    var validacion = true;

    //Verificar espacios en blanco --------------------------------------
    //Verificar campo username
    if (usuario == '') {

        Swal.fire({
            icon: "error",
            text: "Campo de usuario está vacío",
            showConfirmButton: false,
        });

        $('#inputUsuario').addClass('is-invalid');

        validacion = false;
    } else {
        $('#inputUsuario').removeClass('is-invalid');
    }

    //Verificar correo
    if (correo == '') {

        Swal.fire({
            icon: "error",
            text: "Campo de correo está vacío",
            showConfirmButton: false,
        });

        $('#inputCorreo').addClass('is-invalid');

        validacion = false;
    } else {
        $('#inputCorreo').removeClass('is-invalid');
    }

    //Verificar contraseña 1
    if (pass == '') {

        Swal.fire({
            icon: "error",
            text: "Campo de contraseña está vacío",
            showConfirmButton: false,
        });

        $('#inputPass').addClass('is-invalid');

        validacion = false;
    } else {
        $('#inputPass').removeClass('is-invalid');

    }

    //Verificar contraseña 2
    if (passConfirm == '') {

        Swal.fire({
            icon: "error",
            text: "Campo de verificar contraseña está vacío",
            showConfirmButton: false,
        });

        $('#inputPassConfirm').addClass('is-invalid');

        validacion = false;
    } else {
        $('#inputPassConfirm').removeClass('is-invalid');

    }

    //verificar minimo de caracteres en el usuario
    if (usuario.length < 5) {

        validacion = false;
        Swal.fire({
            icon: "error",
            text: "Ingresar nombre de usuario al menos 5 caracteres",
            showConfirmButton: false,
        });
        $('#inputUsuario').addClass('is-invalid');

    } else {
        $('#inputUsuario').removeClass('is-invalid');

    }

    //verificar si el correo es valido con expresion regular standart por funcion
    function validateEmail(correo) {
        return correo.match(
            /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
        );
    };

    if (!validateEmail(correo)) {

        validacion = false;
        Swal.fire({
            icon: "error",
            text: "Debe ingresar un correo válido",
            showConfirmButton: false,
        });
        $('#inputCorreo').addClass('is-invalid');
    } else {
        $('#inputCorreo').removeClass('is-invalid');
    }

    //****************************FILTROS DE CONTRASEÑA ****************************
    //Verificar si los dos campos de contraseña coinciden  --------------------------
    if (pass != passConfirm) {

        validacion = false;
        Swal.fire({
            icon: "error",
            text: "Las contraseñas no coinciden",
            showConfirmButton: false,
        });
        $('#inputPass').addClass('is-invalid');

    } else {
        $('#inputPass').removeClass('is-invalid');

    }

    //al menos 8 caracteres
    if (pass.length < 8) {

        validacion = false;
        Swal.fire({
            icon: "error",
            text: "La contraseña debe tener al menos 8 caracteres",
            showConfirmButton: false,
        });
        $('#inputPass').addClass('is-invalid');

    } else {
        $('#inputPass').removeClass('is-invalid');

    }

    //verificar si se usa al menos una mayuscula
    if (!pass.match(/[A-Z]/g)) {

        validacion = false;
        Swal.fire({
            icon: "error",
            text: "La contraseña debe contener al menos una mayúscula",
            showConfirmButton: false,
        });
        $('#inputPass').addClass('is-invalid');

    } else {
        $('#inputPass').removeClass('is-invalid');

    }

    //verificar si la contraseña contiene un caracter especial: @,#,-,*,$,%,&
    if (!pass.match(/["@,#,-,*,$,%,&"]/g)) {

        validacion = false;
        Swal.fire({
            icon: "error",
            text: "La contraseña debe contener al menos un caracter especial",
            showConfirmButton: false,
        });
        $('#inputPass').addClass('is-invalid');

    } else {
        $('#inputPass').removeClass('is-invalid');

    }

    //la contraseña debe contener al menors x numeros
    if (!pass.match(/[0-9]{2}/g)) {

        validacion = false;
        Swal.fire({
            icon: "error",
            text: "La contraseña debe contener al menos dos numeros",
            showConfirmButton: false,
        });
        $('#inputPass').addClass('is-invalid');

    } else {
        $('#inputPass').removeClass('is-invalid');

    }

    //Verificacion final para hacer el post a php y agregar usuario en SQL
    if (validacion == true) {

        var data = {
            nombreUsuario: usuario,
            correo: correo,
            PasswordHash: pass
        };

        console.log(data);

        $('#correo').on('blur', function () {
            var email = $(this).val();
            if (email) {
                $.post({
                    url: '@Url.Action("CheckEmail", "Accounts")',
                    data: { correo: email },
                    success: function (disponible) {
                        if (disponible) {

                            // Realizar la solicitud POST
                            $.post({
                                url: '@Url.Action("Register", "Accounts")', // Ruta de la acción en el controlador
                                data: data,
                                success: function (response) {
                                    alert("Registro exitoso.");
                                    window.location.href = '@Url.Action("Index", "Home")'; // Redirigir en caso de exito
                                },
                                error: function (xhr, status, error) {
                                    console.log("Error:", error);
                                    alert("Error al registrar usuario.");
                                }
                            });

                        } else {

                            Swal.fire({
                                icon: "error",
                                text: "Correo ya registrado",
                                showConfirmButton: false,
                            });
                        }
                    }
                });
            }
        });



    };
});
