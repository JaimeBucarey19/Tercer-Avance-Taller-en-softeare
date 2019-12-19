function evento(estudiante_id){
	//FUNCION QUE CARGA LA INFORMACION DEL PROSPECTO EN UN MARCO
	location.href='/gae/gestion_casos/evento/';
};

var rules_basic = {
  condition: 'AND',
  rules: [{
    id: 'tag__descripcion',
    operator: 'equal',
    value: 'Niveles de Logros'
  }, /*{
    condition: 'OR',
    rules: [{
      id: 'sede__vigente',
      operator: 'equal',
      value: 2
    }, {
      id: 'category',
      operator: 'equal',
      value: 1
    }]*/
  ]  
};
var filters;
$.ajax(
			  {
            url: '../../management/load_entities/',            
            async: false,
            dataType: 'json',
            type: 'post',
            success: function(data){
				filters=data;
            },
            
        });
$('#builder-basic').queryBuilder(

{
  plugins: ['bt-tooltip-errors'],
  filters:filters,  

  rules: rules_basic
});
// reset builder
$('.reset').on('click', function() {
  var target = $(this).data('target');
  
  $('#builder-'+target).queryBuilder('reset');
});
//set rules
$('.set-json').on('click', function() {
  var target = $(this).data('target');
  var rules = window['rules_'+target];
  
  $('#builder-'+target).queryBuilder('setRules', rules);
});
//get rules
$('.parse-json').on('click', function() {
  var target = $(this).data('target');
  var result = $('#builder-'+target).queryBuilder('getRules');
  
  if ($.isEmptyObject(result)) {
    bootbox.alert({
      title: $(this).text(),
      message: '<pre class="code-popup">' + 'Debe tener al menos una regla' + '</pre>'
    });
  }
    $.ajax(
			  {
            url: '../../management/load_data/',            
			data:{'json_dict':JSON.stringify(result,null,2)},
            async: false,
            dataType: 'json',
            type: 'post',
            success: function(data){
				//alert(data);
				
				var id_body_table=$('#id_body_table');
				id_body_table.children().remove();
				$.each(data['aaData'], function(id, object){	
				var tr=$("<tr>");			
				tr.append('<td><input type="checkbox"  ident="'+object[8]+'" checked class="i-checks class_tutoriado" name="input[]"></td>');				
				tr.append("<td>"+object[1]+"</td>");							
				tr.append("<td>"+object[2]+"</td>");				
				tr.append("<td>"+object[3]+"</td>");
				tr.append("<td>"+object[4]+"</td>");				
				tr.append("<td>"+object[5]+"</td>");
				tr.append("<td>"+object[6]+"</td>");
				tr.append("<td>"+object[8]+"</td>");
				tr.append("<td>"+object[9]+"</td>");
				id_body_table.append(tr);
			});
            },
            
        })
});
$('#btn-reset').on('click', function() {
  $('#builder-basic').queryBuilder('reset');
});

$('#btn-set').on('click', function() {
  $('#builder-basic').queryBuilder('setRules', rules_basic);
});

$('#btn-get').on('click', function() {
  var result = $('#builder-basic').queryBuilder('getRules');
  
 // if (!$.isEmptyObject(result)) {
  //  alert(JSON.stringify(result, null, 2));
  //}
  $.ajax(
			  {
            url: '../../management/load_data/',            
			data:{'json_dict':result},
            async: false,
            dataType: 'json',
            type: 'post',
            success: function(data){
				alert(data);
            },
            
        });
});
$('.parse-sql').on('click', function() {
  var target = $(this).data('target');
  var result = $('#builder-'+target).queryBuilder('getSQL', $(this).data('stmt'));
  
  if (result.sql.length) {
    bootbox.alert({
      title: $(this).text(),
      message: '<pre class="code-popup">' + result.sql + (result.params ? '\n\n' + JSON.stringify(result.params, null, 2) : '') + '</pre>'
    });
  }
});