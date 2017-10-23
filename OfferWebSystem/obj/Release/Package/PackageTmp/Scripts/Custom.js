
$(document).ready(function () {
    $('.parallax').parallax();
    $(document).on("click", "#btnNewPost", function () {
        var url = $(this).attr("data-url");
        $('#modal-post-loader').removeClass('hide');
        $('#modal-post-loader').addClass('active');
        $.get(url, function (data) {
            $('#modal-post-detail').html(data);
            $.validator.unobtrusive.parse($('#modal-post-detail'));
            $('#modal-post-loader').addClass('hide');
        });
    });

    $(document).on("click", ".editOutlet", function () {
        var url = $(this).attr("data-url");
        $('#modal1').openModal({
            opacity: 0.5,
            in_duration: 350,
            out_duration: 250,
            ready: undefined,
            complete: undefined,
            dismissible: true
        });
        $('#modal-post-detail').empty();
        $('#modal-post-loader').removeClass('hide');
        $('#modal-post-loader').addClass('active');
        var id = $(this).attr('data-id'); // the id that's given to each button in the list
        $.get(url + '/' + id, function (data) {
            $('#modal-post-detail').html(data);
            $.validator.unobtrusive.parse($('#modal-post-detail'));
            $('#modal-post-loader').addClass('hide');
        });
    });

    $(document).on("click", "#newOutlet", function () {
        var url = $(this).attr("data-url");
        $('#modal-post-detail').empty();
        $('#modal-post-loader').removeClass('hide');
        $('#modal-post-loader').addClass('active');
        $.get(url, function (data) {
            $('#modal-post-detail').html(data);
            $.validator.unobtrusive.parse($('#modal-post-detail'));
            $('#modal-post-loader').addClass('hide');
        });
    });

    $(document).on("click", ".editLocation", function () {
        var url = $(this).attr("data-url");
        $('#modal-post-detail').empty();
        $('#modal-post-loader').removeClass('hide');
        $('#modal-post-loader').addClass('active');
        var id = $(this).attr('data-id'); // the id that's given to each button in the list
        $.get(url + '/' + id, function (data) {
            $('#modal-post-detail').html(data);
            $.validator.unobtrusive.parse($('#modal-post-detail'));
            $('#modal-post-loader').addClass('hide');
        });
    });

    $(document).on("click", "#newLocation", function () {
        var url = $(this).attr("data-url");
        $('#modal-post-loader').removeClass('hide');
        $('#modal-post-loader').addClass('active');
        $('#modal-post-detail').empty();
        $.get(url, function (data) {
            $('#modal-post-detail').html(data);
            $.validator.unobtrusive.parse($('#modal-post-detail'));
            $('#modal-post-loader').addClass('hide');
        });
    });

    $(document).on("click", ".editCategory", function () {
        var url = $(this).attr("data-url");
        $('#modal-post-detail').empty();
        $('#modal-post-loader').removeClass('hide');
        $('#modal-post-loader').addClass('active');
        var id = $(this).attr('data-id'); // the id that's given to each button in the list
        $.get(url + '/' + id, function (data) {
            $('#modal-post-detail').html(data);
            $.validator.unobtrusive.parse($('#modal-post-detail'));
            $('#modal-post-loader').addClass('hide');
        });
    });

    $(document).on("click", "#newCategory", function () {
        var url = $(this).attr("data-url");
        $('#modal-post-loader').removeClass('hide');
        $('#modal-post-loader').addClass('active');
        $('#modal-post-detail').empty();
        $.get(url, function (data) {
            $('#modal-post-detail').html(data);
            $.validator.unobtrusive.parse($('#modal-post-detail'));
            $('#modal-post-loader').addClass('hide');
        });
    });




    $(document).on("click", ".chkCategories , .chkLocation , .chkPostActive", function () {
        var actionUrl = $(this).attr("data-url");
        $('#post-list').empty();
        var searchText = $('#post-search').val();
        var CategoryId = new Array();
        var LocationId = new Array();
        var PostStatus = new Array();
        $('.chkCategories:checkbox:checked').each(function () {
            CategoryId.push($(this).val());
        });

        $('.chkLocation:checkbox:checked').each(function () {
            LocationId.push($(this).val());
        });

        $('.chkPostActive:checkbox:checked').each(function () {
            PostStatus.push($(this).val());
        });

        $('#post-list-loader').removeClass('hide');
        $('#post-list-loader').addClass('active');
        $.ajax({
            cache: false,
            type: "GET",
            url: actionUrl,
            traditional: true,
            data: { "CategoryId": CategoryId, "LocationId": LocationId, "SearchWord": searchText, "PostStatus": PostStatus },
            success: function (data) {

                $('#post-list').html(data);
                $('#post-list-loader').addClass('hide');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#post-list-loader').addClass('hide');
                alert('Failed to load Information');
            }
        });
    });

    //$(document).on("click", ".chkLocation", function () {
    //    var actionUrl = $(this).attr("data-url");
    //    $('#post-list').empty();
    //    var searchText = $('#post-search').val();
    //    var CategoryId = new Array();
    //    var LocationId = new Array();
    //    $('.chkCategories:checkbox:checked').each(function () {
    //        CategoryId.push($(this).val());
    //    });

    //    $('.chkLocation:checkbox:checked').each(function () {
    //        LocationId.push($(this).val());
    //    });
    //    $('#post-list-loader').removeClass('hide');
    //    $('#post-list-loader').addClass('active');
    //    $.ajax({
    //        cache: false,
    //        type: "GET",
    //        url: actionUrl,
    //        traditional: true,
    //        data: { "CategoryId": CategoryId, "LocationId": LocationId, "SearchWord": searchText },
    //        success: function (data) {
    //            $('#post-list').html(data);
    //            $('#post-list-loader').addClass('hide');
    //        },
    //        error: function (xhr, ajaxOptions, thrownError) {
    //            $('#post-list-loader').addClass('hide');
    //            alert('Failed to load Information');
    //        }
    //    });
    //});

    $(document).on("keypress", "#post-search", function (e) {
        if (e.which == 13) {
            var actionUrl = $(this).attr("data-url");
            $('#post-list').empty();
            var searchText = $('#post-search').val();
            var CategoryId = new Array();
            var LocationId = new Array();
            var PostStatus = new Array();
            $('.chkCategories:checkbox:checked').each(function () {
                CategoryId.push($(this).val());
            });

            $('.chkLocation:checkbox:checked').each(function () {
                LocationId.push($(this).val());
            });

            $('.chkPostActive:checkbox:checked').each(function () {
                PostStatus.push($(this).val());
            });

            $('#post-list-loader').removeClass('hide');
            $('#post-list-loader').addClass('active');
            $.ajax({
                cache: false,
                type: "GET",
                url: actionUrl,
                traditional: true,
                data: { "CategoryId": CategoryId, "LocationId": LocationId, "SearchWord": searchText, "PostStatus": PostStatus },
                success: function (data) {

                    $('#post-list').html(data);
                    $('#post-list-loader').addClass('hide');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $('#post-list-loader').addClass('hide');
                    alert('Failed to load Information');
                }
            });
        }
    });


    $(document).on("click", ".chkOutletLocation", function () {
        var actionUrl = $(this).attr("data-url");
        $('#outlet-list').empty();
        var searchText = $('#outlet-search').val();

        var LocationId = new Array();

        $('.chkOutletLocation:checkbox:checked').each(function () {
            LocationId.push($(this).val());
        });
        $('#outlet-list-loader').removeClass('hide');
        $('#outlet-list-loader').addClass('active');
        $.ajax({
            cache: false,
            type: "GET",
            url: actionUrl,
            traditional: true,
            data: { "LocationId": LocationId, "SearchWord": searchText },
            success: function (data) {
                $('#outlet-list').html(data);
                $('#outlet-list-loader').addClass('hide');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#outlet-list-loader').addClass('hide');
                alert('Failed to load Information');
            }
        });
    });


    $(document).on("keypress", "#outlet-search", function (e) {
        if (e.which == 13) {
            var actionUrl = $(this).attr("data-url");
            $('#outlet-list').empty();
            var searchText = $('#outlet-search').val();

            var LocationId = new Array();

            $('.chkOutletLocation:checkbox:checked').each(function () {
                LocationId.push($(this).val());
            });
            $('#outlet-list-loader').removeClass('hide');
            $('#outlet-list-loader').addClass('active');
            $.ajax({
                cache: false,
                type: "GET",
                url: actionUrl,
                traditional: true,
                data: { "LocationId": LocationId, "SearchWord": searchText },
                success: function (data) {
                    $('#outlet-list').html(data);
                    $('#outlet-list-loader').addClass('hide');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $('#outlet-list-loader').addClass('hide');
                    alert('Failed to load Information');
                }
            });
        }
    });




    $(document).on("click", ".chkUserType", function () {
        var actionUrl = $(this).attr("data-url");
        $('#outlet-list').empty();
        var searchText = $('#user-search').val();

        var UserType = new Array();

        $('.chkUserType:checkbox:checked').each(function () {
            UserType.push($(this).val());
        });
        $('#user-list-loader').removeClass('hide');
        $('#user-list-loader').addClass('active');
        $.ajax({
            cache: false,
            type: "GET",
            url: actionUrl,
            traditional: true,
            data: { "UserType": UserType, "SearchWord": searchText },
            success: function (data) {
                $('#user-full-list').html(data);
                $('#user-list-loader').addClass('hide');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#outlet-list-loader').addClass('hide');
                alert('Failed to load Information');
            }
        });
    });


    $(document).on("keypress", "#user-search", function (e) {
        if (e.which == 13) {
            var actionUrl = $(this).attr("data-url");
            $('#outlet-list').empty();
            var searchText = $('#user-search').val();

            var UserType = new Array();

            $('.chkUserType:checkbox:checked').each(function () {
                UserType.push($(this).val());
            });
            $('#user-list-loader').removeClass('hide');
            $('#user-list-loader').addClass('active');
            $.ajax({
                cache: false,
                type: "GET",
                url: actionUrl,
                traditional: true,
                data: { "UserType": UserType, "SearchWord": searchText },
                success: function (data) {
                    $('#user-full-list').html(data);
                    $('#user-list-loader').addClass('hide');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $('#outlet-list-loader').addClass('hide');
                    alert('Failed to load Information');
                }
            });
        }
    });

});
