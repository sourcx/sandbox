function getAbDataByID(id) {
  var abObj = {};
  abObj.result = false;

  ref = new ActionReference();
  ref.putIdentifier(charIDToTypeID( 'Lyr ' ), parseInt(id));

  var desc = executeActionGet(ref);

  var ab_actDesc = desc.getObjectValue(stringIDToTypeID('artboard'));
  var abrect_desc = ab_actDesc.getObjectValue(stringIDToTypeID('artboardRect'));
  // get bounds of artboard.
  abObj.top = parseInt(abrect_desc.getUnitDoubleValue(charIDToTypeID('Top ')))
  abObj.left = parseInt(abrect_desc.getUnitDoubleValue(charIDToTypeID('Left')));
  abObj.bottom = parseInt(abrect_desc.getUnitDoubleValue(charIDToTypeID('Btom')));
  abObj.right = parseInt(abrect_desc.getUnitDoubleValue(charIDToTypeID('Rght')));

  // add the 4 values together, and if they are 0  then I know its not an actual artboard.
  var checVal = (abObj.top+abObj.left+abObj.bottom+abObj.right);
  if (checVal === 0)  return abObj;

  abObj.width = abObj.right - abObj.left;
  abObj.height = abObj.bottom - abObj.top

  alert(abObj.top)
  // abObj.rulerOrigin = getActiveDocRulerOrigin();
  //
  // var lt = -abObj.rulerOrigin[0] + abObj.left;
  // var tp = -abObj.rulerOrigin[1] + abObj.top;
  // var rt = abObj.width + lt ;
  // var bt = abObj.height + tp ;
  //
  // abObj.hguides = getGuidesWithinBounds(aDoc,abObj.left,abObj.top,abObj.right,abObj.bottom)[0];
  // abObj.vguides = getGuidesWithinBounds(aDoc,abObj.left,abObj.top,abObj.right,abObj.bottom)[1];
  // abObj.name = desc.getString(charIDToTypeID( 'Nm  ' ));
  // abObj.id = desc.getInteger(stringIDToTypeID( 'layerID' ));
  // abObj.index = desc.getInteger(charIDToTypeID( 'ItmI' ));
  // abObj.result = true;
  // abObj.allData = "Name: " + abObj.name + "\nID: " + abObj.id + "\nIndex: " + abObj.index + "\nTop: " + abObj.top + "\nLeft: " + abObj.left + "\nBottom: " + abObj.bottom + "\nRight: " + abObj.right + "\nWidth: " + abObj.width + "\nHeight: " + abObj.height + "\nHorizontal Guides: "  + abObj.hguides + "\nVertical Guides: " + abObj.vguides
  //
  return abObj;
}

// alert('ok');
function getActiveLayerID() {
  var ref = new ActionReference();
  ref.putProperty( charIDToTypeID("Prpr") , charIDToTypeID( "LyrI" ));
  ref.putEnumerated( charIDToTypeID("Lyr "), charIDToTypeID("Ordn"), charIDToTypeID("Trgt") );
  return executeActionGet(ref).getInteger( stringIDToTypeID( "layerID" ) );
}

function getAllArtboardsMeh() {
  try {
    var ab = [];
    var theRef = new ActionReference();
    theRef.putProperty(charIDToTypeID('Prpr'), stringIDToTypeID("artboards"));
    theRef.putEnumerated(charIDToTypeID('Dcmn'), charIDToTypeID('Ordn'), charIDToTypeID('Trgt'));
    var getDescriptor = new ActionDescriptor();
    getDescriptor.putReference(stringIDToTypeID("null"), theRef);
    var abDesc = executeAction(charIDToTypeID("getd"), getDescriptor, DialogModes.NO).getObjectValue(stringIDToTypeID("artboards"));
    var abCount = abDesc.getList(stringIDToTypeID('list')).count;
    // alert(abCount);

    if (abCount > 0) {
      for (var i = 0; i < abCount; ++i) {
        var listItem = abDesc.getList(stringIDToTypeID('list')).getObjectValue(i);
        // alert(listItem);
        var rect = listItem.getObjectValue(stringIDToTypeID('artboardRect'));
        // var abTopIndex = listItem.getInteger(stringIDToTypeID("top"));

        // alert(rect);
        // get bounds of artboard.

        alert(listItem.getInteger(charIDToTypeID('Top ')));
        // alert(rect.getUnitDoubleValue(charIDToTypeID('Left')));
        // alert(rect.getUnitDoubleValue(charIDToTypeID('Btom')));
        // alert(rect.getUnitDoubleValue(charIDToTypeID('Rght')));

        // var abTopIndex = listItem.getInteger(stringIDToTypeID("top"));

        // ab.push(abTopIndex);
      }
    }

    return [abCount, ab];
  } catch (e) {
    alert(e.line + '\n' + e.message);
  }
}

function getAllArtboards () {
  var originalRulerUnits = app.preferences.rulerUnits;
  app.preferences.rulerUnits = Units.PIXELS;

  // the file;
  var myDocument = app.activeDocument;
  // get number of layers;
  var ref = new ActionReference();
  ref.putEnumerated( charIDToTypeID("Dcmn"), charIDToTypeID("Ordn"), charIDToTypeID("Trgt") );

  var applicationDesc = executeActionGet(ref);
  var theNumber = applicationDesc.getInteger(stringIDToTypeID("numberOfLayers"));
  // applicationDesc.getInteger(stringIDToTypeID("numberOfLayers"));

  // process the layers;
  var theLayers = new Array;
  for (var m = 0; m <= theNumber; m++) {

    try {
      var ref = new ActionReference();
      ref.putIndex( charIDToTypeID( "Lyr " ), m);
      var layerDesc = executeActionGet(ref);

      // if artboard;
      if (layerDesc.getBoolean(stringIDToTypeID("artboardEnabled")) == true) {
        var artBoardRect = layerDesc.getObjectValue(stringIDToTypeID("artboard")).getObjectValue(stringIDToTypeID("artboardRect"));
        var theName = layerDesc.getString(stringIDToTypeID('name'));
        var theID = layerDesc.getInteger(stringIDToTypeID('layerID'));
        alert("Name: " + theName + ". id: " + theID);
        theLayers.push([theName, theID, [artBoardRect.getUnitDoubleValue(stringIDToTypeID("left")), artBoardRect.getUnitDoubleValue(stringIDToTypeID("top")), artBoardRect.getUnitDoubleValue(stringIDToTypeID("right")), artBoardRect.getUnitDoubleValue(stringIDToTypeID("bottom"))]])
      };
    }
    catch (e) {};
  };

  alert(theLayers);
}

function selectLayerByID(id, add){
  add = (add == undefined) ? add = false : add;
  var ref = new ActionReference();
  ref.putIdentifier(charIDToTypeID('Lyr '), id);
  var desc = new ActionDescriptor();
  desc.putReference(charIDToTypeID('null'), ref);
  if(add){
    desc.putEnumerated(stringIDToTypeID('selectionModifier'), stringIDToTypeID('selectionModifierType'), stringIDToTypeID('addToSelection'));
  }
  desc.putBoolean(charIDToTypeID('MkVs'), false);
  executeAction(charIDToTypeID('slct'), desc, DialogModes.NO);
}



//get the list of multiple selected layers
//http://www.nekomataya.info/nekojyarashi/wiki.cgi?photoshop%CA%A3%BF%F4%A5%BB%A5%EC%A5%AF%A5%C8

function getSelectedLayers(){
	var idGrp = stringIDToTypeID( "groupLayersEvent" );
	var descGrp = new ActionDescriptor();
	var refGrp = new ActionReference();
	refGrp.putEnumerated(charIDToTypeID( "Lyr " ),charIDToTypeID( "Ordn" ),charIDToTypeID( "Trgt" ));
	descGrp.putReference(charIDToTypeID( "null" ), refGrp );
	executeAction( idGrp, descGrp, DialogModes.ALL );
	var resultLayers=new Array();
	for (var ix=0;ix<app.activeDocument.activeLayer.layers.length;ix++){resultLayers.push(app.activeDocument.activeLayer.layers[ix])}
	var id8 = charIDToTypeID( "slct" );
    var desc5 = new ActionDescriptor();
    var id9 = charIDToTypeID( "null" );
    var ref2 = new ActionReference();
    var id10 = charIDToTypeID( "HstS" );
    var id11 = charIDToTypeID( "Ordn" );
    var id12 = charIDToTypeID( "Prvs" );
    ref2.putEnumerated( id10, id11, id12 );
	desc5.putReference( id9, ref2 );
	executeAction( id8, desc5, DialogModes.NO );
	return resultLayers;
}


var newName = prompt("layer name", "new name");

var layers = getSelectedLayers();
for (var i = 0; i < layers.length; i ++){
	layers[i].name = newName;
}


function getArtboardOfActivelayer() {
  activeLayerId = getActiveLayerID();
  activeLayer = selectLayerByID(activeLayerId, false);
  alert(activeLayer);
  // if (layer.getBoolean(stringIDToTypeID("artboardEnabled")) == true) {
//     alert('has artboardEnabled');
//   } else {
//     alert('no artboardEnabled');
//   }
//   var layerName = layer.getString(stringIDToTypeID('name'));
  // alert(layerName);
}


// getAbDataByID(getActiveLayerID());
// alert("active layer: " + getActiveLayerID());
// getArtboardOfActivelayer();
