var ref = new ActionReference();
ref.putProperty( charIDToTypeID("Prpr") , stringIDToTypeID( "artboards" ));
ref.putEnumerated( charIDToTypeID("Dcmn"), charIDToTypeID("Ordn"), charIDToTypeID("Trgt") );
var desc = executeActionGet(ref);

alert(desc);

// geen artboards reference te vinden in cs6, krijgt exception.
