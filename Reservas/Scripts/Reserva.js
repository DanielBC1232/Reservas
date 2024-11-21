
function formatearHora(hora) {
    // Si la hora está en formato hh:mm, le agregamos los segundos ":00"
    if (hora.length === 5) {
        return hora + ":00";
    }
    return hora;  // Si ya está en formato hh:mm:ss, lo dejamos tal cual
}

function Reservar(nombre,ids) {

    var tiempoInicio = $("#tiempoInicio").val();
    var tiempoCierre = $("#tiempoCierre").val();
    var fecha = $("#fecha").val();
    var idSala = ids;

    tiempoInicioF = formatearHora(tiempoInicio);
    tiempoCierreF = formatearHora(tiempoCierre);


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
                alert("La sala está disponible.");

                //crear reserva
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
                alert("La sala no está disponible en la fecha y hora seleccionadas.");
            }
        }
    });

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
                            <button class="btn btn-sm btn-primary" onclick="Reservar('${sala.nombreSala}', '${sala.Idsala}')">Reservar</button>
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