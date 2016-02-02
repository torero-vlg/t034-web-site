﻿define(['datatables'], function (dataTables) {

    //$('#table_id').DataTable(
    //        {
    //            'language': {
    //                'url': 'Content/DataTables-1.10.2/dataTables.russian.langs'
    //            }
    //        });

    return {
        Initialize: function () {
            $('#table_id').DataTable(
            {
                'language': {
                    'url': 'Content/DataTables-1.10.2/dataTables.russian.langs'
                }
            });
        }
    }
});
