let loadMoreBtn = document.getElementById("load-more");
let productList = document.getElementById("prduct-list");
let skip = 3;

if (loadMoreBtn) {
    loadMoreBtn.addEventListener("click", function () {
        console.log("sssss")
        fetch('Blogs/PastialBlogs?skip=' + skip, {
            method: 'POST'
        })
            .then((response) => response.text())
            .then((data) => {
                console.log(data)
                productList.innerHTML += data;
                skip += 3;
                let productCount = document.getElementById("prduct-count").value;
                console.log(productCount)
                if (skip >= productCount)
                    loadMoreBtn.remove();
            });
    })
}

var searchInput = document.getElementById("search-corses");
if (searchInput) {
    searchInput.addEventListener("keyup", function () {     

        let text = this.value
        let productList = document.querySelector("#product-list")

        fetch('Courses/Search?searchText=' + text)
            .then((response) => response.text())
            .then((data) => {
                console.log(data)
                productList.innerHTML = data
            });

    });
}



