
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


function validarInputs(tiempoInicio, tiempoCierre, fecha, aperturaV, cierreV) {
    if (!tiempoInicio || !tiempoCierre || !fecha) {
        Swal.fire({ icon: "error", text: "Campos de rango de horas o fecha vacíos." });
        return false;
    }

    if (horaToMinutos(tiempoInicio) >= horaToMinutos(tiempoCierre)) {
        Swal.fire({ icon: "error", text: "La hora de inicio es mayor que la de cierre." });
        return false;
    }

    if (horaToMinutos(tiempoInicio) < aperturaV || horaToMinutos(tiempoCierre) > cierreV) {
        Swal.fire({ icon: "error", text: "El rango de tiempo está fuera del horario de la sala." });
        return false;
    }

    let fechaActual = new Date();
    let fechaReserva = new Date(fecha);
    if (fechaReserva <= fechaActual) {
        Swal.fire({ icon: "error", text: "No puedes reservar en una fecha pasada." });
        return false;
    }

    return true;
}

//verificar antes de insertar conflictos con reservas con la misma fecha y horas
function verificarDisponibilidad(nombre, tiempoInicio, tiempoCierre, fecha, callback) {
    $.ajax({
        url: '/Salas/VerificarReserva',
        method: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({ nombreSala: nombre, horaInicio: tiempoInicio, horaCierre: tiempoCierre, fecha: fecha }),
        success: function (respuesta) {
            if (respuesta.success === true) {
                callback(true);
            } else {
                Swal.fire({
                    icon: "error",
                    text: "La sala no está disponible en la fecha y horas seleccionadas.",
                });
                callback(false);
            }
        }

    });
}

function ReservarBtn(nombre, ids, aperturaV, cierreV) {
    var tiempoInicio = $("#tiempoInicio").val();
    var tiempoCierre = $("#tiempoCierre").val();
    var fecha = $("#fecha").val();

    // validaciones de inputs
    if (!validarInputs(tiempoInicio, tiempoCierre, fecha, aperturaV, cierreV)) {
        return;
    }

    // verificar disponibilidad
    verificarDisponibilidad(nombre, tiempoInicio, tiempoCierre, fecha, function (isAvailable) {
        if (isAvailable) {
            Reservar(nombre, ids, fecha, tiempoInicio, tiempoCierre);
        }
    });
}

function Reservar(nombre, ids, fecha, tiempoInicio, tiempoCierre) {
    $.ajax({
        url: '/Reservas/Create',
        method: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify({
            fecha: fecha,
            horaInicio: tiempoInicio,
            horaFin: tiempoCierre,
            nombreSala: nombre,
            Idsala: ids
        }),
        success: function () {
            Swal.fire({ icon: "success", text: `Sala ${nombre} reservada exitosamente.` });
        },
        error: function () {
            Swal.fire({ icon: "error", text: "Error al reservar la sala." });
        }
    });
}

//extraer numero de horas
function extractHour(time) {
    var hours = time.hours;
    return hours;
}


$(document).ready(function () {

    function filtroS() {
        var inputValue = $("#filtro").val().toLowerCase(); // Valor del campo de texto, pasa a minuscula

        $("#tableBody tr").filter(function () {

            // buscar en todas las columnas
            var textMatch = $(this).text().toLowerCase().indexOf(inputValue) > -1;

            //mostrar coincidencias
            $(this).toggle(textMatch);
        });
    }

    // filtro basado en el campo de texto
    $("#filtro").on("keyup", function () {
        filtroS();
    });

});

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
                        <tr class="align-middle" id="tablaFiltroTr">
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
                           <button class="btn btn-sm btn-primary" onclick="ReservarBtn('${sala.nombreSala}', '${sala.Idsala}', 
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