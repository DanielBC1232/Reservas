
function formatearHora(hora) {
    // Si la hora está en formato hh:mm, le agregamos los segundos ":00"
    if (hora.length === 5) {
        return hora + ":00";
    }
    return hora;
}

function horaToMinutos(hora) {
    var horaParts = hora.split(":");
    var horas = parseInt(horaParts[0]);
    var minutos = parseInt(horaParts[1]);
    return horas * 60 + minutos;
}

function Reservar(nombre, ids, aperturaV, cierreV) {

    var tiempoInicio = $("#tiempoInicio").val();
    var tiempoCierre = $("#tiempoCierre").val();
    var fecha = $("#fecha").val();
    var idSala = ids;

    //formato de hora para verificar existencia
    tiempoInicioF = formatearHora(tiempoInicio);
    tiempoCierreF = formatearHora(tiempoCierre);

    //inputs de horas
    var inputHoraInicio = horaToMinutos(tiempoInicio);
    var inputHoraCierre = horaToMinutos(tiempoCierre);

    //verificaciones ------------------------------------------------------------

    //var global para verificar varios aspectos antes de agregar a BD
    var verifica = true;

    //verificar que los campos de horas y fecha no esten vacios
    if (tiempoInicio.trim() == "" || tiempoCierre.trim() == "" || fecha.trim() == "") {
        verifica = false;

        Swal.fire({
            icon: "error",
            text: "Campos de rango de horas o fecha vacios.",
            showConfirmButton: false,
        });
        return;
    } else if (tiempoInicio.trim() !== "" && tiempoCierre.trim() !== "" && fecha.trim() !== "") {
        verifica = true;
    }

    //verificar que la hora inicial no sea mayor a la de fin
    if (tiempoInicio >= tiempoCierre) {

        verifica = false;

        Swal.fire({
            icon: "error",
            text: "La hora de inicio es mayor que la hora de cierre.",
            showConfirmButton: false,
        });
        return;
    } else if (tiempoInicio <= tiempoCierre) {
        verifica = true;
    }


    //verificar que las horas se encuentren en el rango de disponibilidad del salon seleccionado
    if (horaToMinutos(tiempoInicio) < aperturaV || horaToMinutos(tiempoCierre) > cierreV) {
        verifica = false;

        Swal.fire({
            icon: "error",
            text: "Las el tiempo que intentas reservar esta fuera del rango de tiempo de la sala.",
            showConfirmButton: false,
        });
        return;
    } else if (horaToMinutos(tiempoInicio) >= aperturaV && horaToMinutos(tiempoCierre) <= cierreV) {
        verifica = true;
    }


    //verificar que la fecha sea igual o superior a la fecha actual
    let fechaActual = new Date();

    if (fecha <= fechaActual) {

        verifica = false;

        Swal.fire({
            icon: "error",
            text: "No puedes reservar con una fecha pasada",
            showConfirmButton: false,
        });
        return;
    } else if (fecha >= fechaActual) {
        verifica = true;
    }

    //verificar antes de insertar conflictos con reservas con la misma fecha U horas
    $.ajax({
        url: '/Salas/VerificarReserva',
        method: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({
            nombreSala: nombre,
            horaInicio: tiempoInicioF,
            horaCierre: tiempoCierreF,
            fecha: fecha
        }),
        success: function (respuesta) {
            //console.log(respuesta);
            if (respuesta === 0) {
                verifica = true;

            } else {
                Swal.fire({
                    icon: "error",
                    text: "La sala no esta disponible en la fecha y horas seleccionadas",
                    showConfirmButton: false,
                });
                verifica = false;
                return;
            }
        }
    });



    if (verifica == true) {

        Swal.fire({
            icon: "success",
            text: "Salon " + nombre + " Reservado",
            showConfirmButton: false,
        });
        
        //crear reserva ****
        
        $.ajax({
            url: '/Reservas/Create',
            method: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify({
                nombreUsuario: "Usuario",
                fecha: fecha,
                horaInicio: tiempoInicio,
                horaFin: tiempoCierre,
                nombreSala: nombre,
                Idsala: idSala
            }),
            success: function (response) {
                console.log(response);
            },
            error: function (error) {
                console.error("Error:", error);
            }
        });



    } else {
        //**
    }
    
}
//extraer numero de horas
function extractHour(time) {
    var hours = time.hours;
    return hours;
}

$(document).ready(function () {

    $.ajax({
        url: '/Salas/GetSala',
        method: 'GET',
        dataType: 'json',
        success: function (respuesta) {
            let sala = respuesta;
            let plantilla = '';

            sala.forEach(sala => {
                //console.log(sala);
                plantilla += `
                        <tr class="align-middle">
                            <td class="text-center">${sala.nombreSala}</td>
                            <td class="text-center">${sala.capacidad}</td>
                            <td class="text-center">${sala.ubicacion}</td>
                            <td class="text-center">${sala.disponibilidadEquipo}</td>
                            <td class="text-center">
                                <input type="text" class="form-control" value="${sala.horaApertura.Hours}:${sala.horaApertura.Minutes.toString().padStart(2, '0')}" readonly />
                            </td>
                            <td class="text-center">
                                <input type="text" class="form-control" value="${sala.horaCierre.Hours}:${sala.horaCierre.Minutes.toString().padStart(2, '0')}" readonly />
                            </td>
                            <td>
                           <button class="btn btn-sm btn-primary" onclick="Reservar('${sala.nombreSala}', '${sala.Idsala}', 
                            '${horaToMinutos(sala.horaApertura.Hours + ':' + sala.horaApertura.Minutes.toString().padStart(2, '0'))}',
                            '${horaToMinutos(sala.horaCierre.Hours + ':' + sala.horaCierre.Minutes.toString().padStart(2, '0'))}')">Reservar
                        </button>

                            <td>
                        </tr>
                    `;
                $('#tableBody').html(plantilla);
            });
        },
        error: function (xhr, status, error) {
            console.error('Error al obtener las salas:', error);
        }
    });
});