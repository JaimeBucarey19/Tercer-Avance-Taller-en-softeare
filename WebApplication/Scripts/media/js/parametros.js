$('#id_region').change(function(){
    $('#id_provincia').html(null);
    $('#id_provincia').append('<option value="" selected="selected">-------</option>');
    $('#id_comuna').html(null);
    $('#id_comuna').append('<option value="" selected="selected">-------</option>');
    var region_id=$('#id_region').val();
    $.ajax({
    url: '{% url parametros.views.get_provincia_by_region %}',
    data: {
        'region' : region_id,
    },
    async:false,
    dataType: 'json',
    type: 'post',
    success: load_provincias,
    });
});
function load_provincias(data){
    var select_provincia=$('#id_provincia');
    $.each(data[0], function(id, object){
        select_provincia.append("<option value='"+object[0]+"'>"+object[1]+"</option>");
    });
}
$('#id_provincia').change(function(){
    $('#id_comuna').html(null);
    $('#id_comuna').append('<option value="" selected="selected">-------</option>');
var provincia_id=$('#id_provincia').val();
    $.ajax({
    url: '{% url parametros.views.get_comuna_by_provincia %}',
    data: {
        'provincia' : provincia_id,
    },
    async:false,
    dataType: 'json',
    type: 'post',
    success: load_comuna,
    });
});
function load_comuna(data)
{
    var select_comuna=$('#id_comuna');
    $.each(data[0], function(id, object){
        select_comuna.append("<option value='"+object[0]+"'>"+object[1]+"</option>");
    });
};
$('#id_comuna').change(function(){
        $.ajax({
            url: '{% url facturacion_cobranza.views.get_prospectos_disp_by_comuna %}',
            data: {
                'id_comuna' : $('#id_comuna option:selected').val()

            },
            dataType: 'json',
            type: 'post',
            success: select_add_prospectos
        });

});
    function select_add_prospectos(data){
        var select_prospectos=$('#myselecttsms');
        $.each(data[0], function(id, object){
            select_prospectos.append("<option value='"+object.detalles[0].prospecto_id+"'>"+object.detalles[0].nombre_o_razon_social+"</option>");
        });
    }
