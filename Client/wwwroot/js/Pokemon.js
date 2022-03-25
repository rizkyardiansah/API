$.ajax({
    url: "https://pokeapi.co/api/v2/pokemon"
}).done((result) => {
    let tbodyContent = "";
    $.each(result.results, (index) => {
        let pokemon = result.results[index];
        tbodyContent += `<tr>
                            <td class="text-center">${index+1}</td>
                            <td class="text-center lead font-weight-normal text-capitalize">${pokemon.name}</td>
                            <td class="text-center"><button class="btn btn-primary" onclick="showDetail('${pokemon.name}')" data-toggle="modal" data-target="#pokemonModal">Detail</button></td>
                         </tr>`
    })
    $("#pokemon-table tbody").html(tbodyContent)
}).fail((error) => {
    console.log(error)
})

function showDetail(pokemonName) {
    $.ajax({
        url: `https://pokeapi.co/api/v2/pokemon/${pokemonName}`,
    }).done(result => {
        $("#pokemonModalTitle").text(pokemonName)

        $("#pokemonModal .modal-body #male-front").attr("src", result.sprites.front_default)

        if (result.sprites.back_default == null) {
            $("#pokemonModal .modal-body #male-back").attr("src", "/images/no-pictures.png")
        } else {
            $("#pokemonModal .modal-body #male-back").attr("src", result.sprites.back_default)
        }

        if (result.sprites.front_female == null) {
            $("#pokemonModal .modal-body #female-front").attr("src", "/images/no-pictures.png")
        } else {
            $("#pokemonModal .modal-body #female-front").attr("src", result.sprites.front_female)
        }

        if (result.sprites.back_female == null) {
            $("#pokemonModal .modal-body #female-back").attr("src", "/images/no-pictures.png")
        } else {
            $("#pokemonModal .modal-body #female-back").attr("src", result.sprites.back_female)
        }

        $("#pokemonModal .modal-body #male-front").show()
        $("#pokemonModal .modal-body #male-back").hide()
        $("#pokemonModal .modal-body #female-front").hide()
        $("#pokemonModal .modal-body #female-back").hide()

        let types = "";
        $.each(result.types, (key, val) => {
            types += `<span class="type-icon type-${val.type.name} mr-2 text-uppercase">${val.type.name}</span>`;
        })
        $("#pokemonModal .modal-body .pokemon-types").html(types);

        let abilities = "";
        $.each(result.abilities, (key, val) => {
            abilities += `<span class="badge badge-pill badge-secondary mr-2">${val.ability.name}</span>`
        })
        $("#pokemonModal .modal-body .pokemon-abilities").html(abilities);

        let maxValue = 200;
        $("#pokemonModal .modal-body .pokemon-status .pokemon-hp").width(`${result.stats[0].base_stat / maxValue * 100}%`);
        $("#pokemonModal .modal-body .pokemon-status .pokemon-hp").attr("aria-valuenow", result.stats[0].base_stat);
        $("#pokemonModal .modal-body .pokemon-status .pokemon-hp").text(result.stats[0].base_stat)

        $("#pokemonModal .modal-body .pokemon-status .pokemon-attack").width(`${result.stats[1].base_stat / maxValue * 100}%`);
        $("#pokemonModal .modal-body .pokemon-status .pokemon-attack").attr("aria-valuenow", result.stats[1].base_stat);
        $("#pokemonModal .modal-body .pokemon-status .pokemon-attack").text(result.stats[1].base_stat)

        $("#pokemonModal .modal-body .pokemon-status .pokemon-defense").width(`${result.stats[2].base_stat / maxValue * 100}%`);
        $("#pokemonModal .modal-body .pokemon-status .pokemon-defense").attr("aria-valuenow", result.stats[2].base_stat);
        $("#pokemonModal .modal-body .pokemon-status .pokemon-defense").text(result.stats[2].base_stat)

        $("#pokemonModal .modal-body .pokemon-status .pokemon-special-attack").width(`${result.stats[3].base_stat / maxValue * 100}%`);
        $("#pokemonModal .modal-body .pokemon-status .pokemon-special-attack").attr("aria-valuenow", result.stats[3].base_stat);
        $("#pokemonModal .modal-body .pokemon-status .pokemon-special-attack").text(result.stats[3].base_stat)

        $("#pokemonModal .modal-body .pokemon-status .pokemon-special-defense").width(`${result.stats[4].base_stat / maxValue * 100}%`);
        $("#pokemonModal .modal-body .pokemon-status .pokemon-special-defense").attr("aria-valuenow", result.stats[4].base_stat);
        $("#pokemonModal .modal-body .pokemon-status .pokemon-special-defense").text(result.stats[4].base_stat)

        $("#pokemonModal .modal-body .pokemon-status .pokemon-speed").width(`${result.stats[5].base_stat / maxValue * 100}%`);
        $("#pokemonModal .modal-body .pokemon-status .pokemon-speed").attr("aria-valuenow", result.stats[5].base_stat);
        $("#pokemonModal .modal-body .pokemon-status .pokemon-speed").text(result.stats[5].base_stat)
    }).fail(error => {
        console.log(error)
    })
}

function showMaleFront() {
    $("#pokemonModal .modal-body #male-front").show()
    $("#pokemonModal .modal-body #male-back").hide()
    $("#pokemonModal .modal-body #female-front").hide()
    $("#pokemonModal .modal-body #female-back").hide()
}
function showMaleBack() {
    $("#pokemonModal .modal-body #male-front").hide()
    $("#pokemonModal .modal-body #male-back").show()
    $("#pokemonModal .modal-body #female-front").hide()
    $("#pokemonModal .modal-body #female-back").hide()
}
function showFemaleFront() {
    $("#pokemonModal .modal-body #male-front").hide()
    $("#pokemonModal .modal-body #male-back").hide()
    $("#pokemonModal .modal-body #female-front").show()
    $("#pokemonModal .modal-body #female-back").hide()
}
function showFemaleBack() {
    $("#pokemonModal .modal-body #male-front").hide()
    $("#pokemonModal .modal-body #male-back").hide()
    $("#pokemonModal .modal-body #female-front").hide()
    $("#pokemonModal .modal-body #female-back").show()
}