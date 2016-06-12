function getActiveLayerID() {
  var ref = new ActionReference();
  // ref.putProperty( charIDToTypeID("Prpr") , charIDToTypeID( "LyrI" ));
  ref.putEnumerated( charIDToTypeID("Lyr "), charIDToTypeID("Ordn"), charIDToTypeID("Trgt") );
  return executeActionGet(ref).getInteger( stringIDToTypeID( "layerID" ) );
}

function getActiveLayerName() {
  var ref = new ActionReference();
  // ref.putProperty( charIDToTypeID("Prpr") , charIDToTypeID( "Nm  " ));
  ref.putEnumerated( charIDToTypeID("Lyr "), charIDToTypeID("Ordn"), charIDToTypeID("Trgt") );
  return executeActionGet(ref).getString( stringIDToTypeID( "name" ) );;
}

function getActiveLayerParentName() {
  var ref = new ActionReference();
  ref.putProperty( charIDToTypeID("Prpr") , stringIDToTypeID("parentName"));
  ref.putEnumerated( charIDToTypeID("Lyr "), charIDToTypeID("Ordn"), charIDToTypeID("Trgt") );
  return executeActionGet(ref).getString( stringIDToTypeID("parentName") );;
}

function getArtboardOfActivelayer() {
  activeLayerId = getActiveLayerID();
  // alert("active layer: " + activeLayerId);

  activeLayerName = getActiveLayerName();
  alert("active layer: " + activeLayerName);

  parentLayerName = getActiveLayerParentName();
  alert("parent layer: " + parentLayerName);

  // var theName = activeLayer.getString(stringIDToTypeID('name'));
  // alert(theName);
  //
  // if (activeLayer.getBoolean(stringIDToTypeID("artboardEnabled")) == true) {
  //   alert("enabled");
  // } else {
  //   alert("not");
  // }
}

getArtboardOfActivelayer();
