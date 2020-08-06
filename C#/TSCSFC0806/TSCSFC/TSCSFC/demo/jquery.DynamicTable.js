 
 (function ($) {
     var rowtmplate = "";
     var arrFocus = [];
     $.fn.DynamicTable = function (options) { //定义插件的名称，这里为userCp 
         var deafult = {
             //以下为该插件的属性及其默认值 
             rowCount: 5, //添加行数 
             identity: 1, //第1列自动编号 
             arrFocus: [2, 1], //第一个单元格设置为焦点 
             rowTmplate: "" //行模版 
         };
         var ops = $.extend(deafult, options);
         rowtmplate = ops.rowTmplate;
         arrFocus = ops.arrFocus;
         $(this).addRow(ops);
     };
     /*通过行模版添加多行至表格最后一行后面*/
     /*count--添加行数*/
     $.fn.addRow = function (options) {
         var deafult = {
             rowCount: 5
         };
         var ops = $.extend(deafult, options);
         var rowData = "";
         var count = ops.rowCount;
         for (var i = 1; i <= count; i++) {
             rowData += rowtmplate;
         }
         $(this).find('tr:last-child').after(rowData);
     };
     /*动态给某列绑定事件,事件被触发时执行fn函数*/
     /*eventName--事件名称;colIndex--列索引(从1开始);fn--触发函数*/
     $.fn.BindEvent = function (options) {
         var deafult = {
             eventName: 'click',
             colIndex: 1,
             fn: function () { alert('你单击了此单元格!') }
         };
         var ops = $.extend(deafult, options);
         eventName = ops.eventName;
         colIndex = ops.colIndex;
         fn = ops.fn;
         $("tr:gt(0) td:nth-child(" + colIndex + ")").bind(eventName, fn);
     };
     /*给某列绑定单击删除事件*/
     /*colIndex--列索引(从1开始)*/
     $.fn.deleteRow = function (options) {
         var deafult = {
             colIndex: 6
         };
         var ops = $.extend(deafult, options);
         var colIndex = ops.colIndex;
         $("tr:gt(0) td:nth-child(" + colIndex + ")").bind("click", function () {
             var obj = $(this).parent(); //获取tr子节点对象 
             if (confirm('您确定要删除吗？')) {
                 obj.remove();
                 obj.parent().Identity();
             }
         });
     };
     /*自动给指定列填充序号*/
     /*colIndex--列索引(从1开始)*/
     $.fn.Identity = function (options) {
         var deafult = {
             colIndex: 1
         };
         var ops = $.extend(deafult, options);
         var colIndex = ops.colIndex;
         var i = 1;
         $(" td:nth-child(" + colIndex + ")").find('a').each(function () {
             $(this).text(i);
             i++;
         });
     };
     /*获取焦点单元格坐标*/
     $.fn.getFocus = function () {
         return arrFocus;
     };
     /*设置焦点单元格坐标*/
     /*rowIndex--行索引(从1开始);colIndex--列索引(从1开始)*/
     $.fn.setFocus = function (options) {
         var deafult = {
             rowIndex: 2,
             colIndex: 1
         };
         var ops = $.extend(deafult, options);
         var rowIndex = ops.rowIndex;
         var colIndex = ops.colIndex;
         arrFocus[0] = rowIndex;
         arrFocus[1] = colIndex;
     };
     /*当某个单元格中输入数据,按Enter键后自动根据输入的值从后台检索数据填充到该行对应列*/
     /*colIndex--第几列输入数据按Enter键触发事件;fn--带参的回调函数*/
     $.fn.AutoFillData = function (options) {
         colIndex = options.colIndex;
         fn = options.fn;
         $("td:nth-child(" + colIndex + ")").bind("keyup", function () {
             var obj = $(this).parent(); //获取tr子节点对象 
             $(this).find('input').each(function () {
                 if (event.keyCode == 13) {
                     var vl = $(this).val();
                     var arr = new Array();
                     arr = fn(vl);
                     var i = 0;
                     obj.find("td").each(function () {
                         $(this).find("input").each(function () {
                             $(this).attr('value', arr[i]);
                             i++;
                         });
                     });
                 }
             });
         });
     };
     /*设置某个单元格为焦点*/
     /*rowIndex--行索引(从1开始);colIndex--列索引(从1开始)*/
     $.fn.setCellsFocus = function (options) {
         var deafult = {
             rowIndex: arrFocus[0],
             colIndex: arrFocus[1]
         };
         var ops = $.extend(deafult, options);
         var rowIndex = ops.rowIndex;
         var colIndex = ops.colIndex;
         $("tr:nth-child(" + rowIndex + ") td:nth-child(" + colIndex + ")").each(function () {
             $(this).find('input').each(function () {
                 $(this)[0].focus();
                 $(this).attr('value', $(this).attr('value'));
                 arrFocus = [];
                 arrFocus.push(rowIndex);
                 arrFocus.push(colIndex); //更新焦点数组值 
             });
         });
     };
     /*设置某个单元格文本值为选中状态*/
     /*rowIndex--行索引(从1开始);colIndex--列索引(从1开始)*/
     $.fn.setCellsSelect = function (options) {
         var deafult = {
             rowIndex: arrFocus[0],
             colIndex: arrFocus[1]
         };
         var ops = $.extend(deafult, options);
         var rowIndex = ops.rowIndex;
         var colIndex = ops.colIndex;
         $("tr:nth-child(" + rowIndex + ") td:nth-child(" + colIndex + ")").each(function () {
             $(this).find('input').each(function () {
                 $(this)[0].select();
             });
         });
     };
     /*某个单元格添加验证功能*/
     /*reg--正则表达式;colIndex--列索引(从1开始);defaultValue--验证失败默认给单元格赋值*/
     $.fn.validationText = function (options) {
         var deafult = {
             reg: /^((\d+\.\d{2})|\d+)$/,
             colIndex: 2,
             defaultValue: 0
         };
         var ops = $.extend(deafult, options);
         var reg = ops.reg;
         var colIndex = ops.colIndex;
         var defaultValue = ops.defaultValue;
         $("tr:gt(0) td:nth-child(" + colIndex + ")").each(function () {
             $(this).find('input').each(function () {
                 //验证 
                 $(this).bind('blur', function () {
                     var vl = $(this).attr('value');
                     if (!reg.test(vl))
                         $(this).attr('value', defaultValue);
                 });
             });
         });
     };
     /*获取表格中的值*/
     $.fn.getValue = function (options) {
         var deafult = {
             rowIndex: 0, //行坐标(从2开始) 
             colIndex: 0 //列坐标(从1开始) 
         };
         var ops = $.extend(deafult, options);
         rowIndex = ops.rowIndex;
         colIndex = ops.colIndex;
         var val = "";
         if (rowIndex == 0) { //获取所有行的数据 
             $('tr:gt(0)').each(function () {
                 $(this).find("td").each(function () {
                     $(this).find("input").each(function () {
                         val += $(this).attr('value') + "&";
                     });
                 });
                 val = val.substring(0, val.length - 1) + "|";
             });
         }
         else {
             if (colIndex == 0) { //获取某行数据 
                 $('tr:nth-child(' + rowIndex + ')').each(function () {
                     $(this).find("td").each(function () {
                         $(this).find("input").each(function () {
                             val += $(this).attr('value') + "&";
                         });
                     });
                     val = val.substring(0, val.length - 1) + "|";
                 });
             }
             else { //获取某个单元格的值 
                 $("tr:nth-child(" + rowIndex + ") td:nth-child(" + colIndex + ")").each(function () {
                     $(this).find('input').each(function () {
                         val += $(this).attr('value');
                     });
                 });
             }
         }
         return val;
     };
     /*某个单元格获取焦点后更新焦点坐标*/
     function CellsFocus() {
         var colCount = $("tr:nth-child(1) td").size(); //获取每行共有多少个单元格 
         $("tr:gt(0) td").each(function () {
             var obj = $(this);
             $(this).find('input').each(function () {
                 $(this).bind('focus', function () {
                     var cellTotal = $('td').index(obj); //获取某单元格的索引 
                     arrFocus[0] = parseInt(cellTotal / colCount) + 1; //第几行 
                     arrFocus[1] = cellTotal % colCount + 1; //第几列 
                 });
             });
         });
     };
 })(jQuery); 
