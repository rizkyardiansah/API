let cards = document.getElementsByClassName("card");
let tab_view = document.getElementsByClassName("tab-view")[0];

for (let i = 0; i < cards.length; i++) {
    cards[i].addEventListener("click", function (e) {
        tab_view.querySelector("h2").innerHTML = cards[i].querySelector("h3").innerHTML;
        tab_view.querySelector("p").innerHTML = cards[i].querySelector("p").innerHTML;
        tab_view.querySelector("p.placeholder").style.display = "none";
    })
}

const animals = [
    { name: "Fluffy", species: "cat", class: { name: "mamalia" } },
    { name: "Nemo", species: "fish", class: { name: "invertebrata" } },
    { name: "Garfield", species: "cat", class: { name: "mamalia" } },
    { name: "Dory", species: "fish", class: { name: "invertebrata" } },
    { name: "Camello", species: "cat", class: { name: "mamalia" } }
]

const OnlyCat = [];
for (let animal of animals) {
    if (animal.species == "cat") {
        OnlyCat.push(animal);
    }
}

console.log(OnlyCat)

animals.forEach((animal) => {
    if (animal.species == "fish") {
        return animal.class.name = "Non-Mamalia";
    }
})

console.log(animals);