const API_URL = "http://localhost:5107/api/v1";

let platos = [];
let categorias = [];
let carrito = [];

// ================== DOMContentLoaded ==================
document.addEventListener("DOMContentLoaded", () => {
    loadCart();
    initCartSidebar();
    fetchCategorias();
    fetchPlatos();
    const currentPage = window.location.pathname.split("/").pop(); // solo el nombre del archivo
    const navLinks = document.querySelectorAll(".navbar-nav .nav-link");

    navLinks.forEach(link => {
        link.classList.remove("active", "inactive"); // limpiar clases

        const linkHref = link.getAttribute("href").split("/").pop(); // obtener nombre del archivo del href

        if (linkHref === currentPage || (currentPage === "" && linkHref === "index.html")) {
            link.classList.add("active"); // amarillo
        } else {
            link.classList.add("inactive"); // gris
        }
    });

    // Filtros y búsqueda
    const searchName = document.getElementById("search-name");
    const filterCategory = document.getElementById("filter-category");
    const filterStatus = document.getElementById("filter-status");
    const sortPrice = document.getElementById("sort-price");

    if (searchName) searchName.addEventListener("input", applyFilters);
    if (filterCategory) filterCategory.addEventListener("change", applyFilters);
    if (filterStatus) filterStatus.addEventListener("change", applyFilters);
    if (sortPrice) sortPrice.addEventListener("change", applyFilters);

    const resetBtn = document.getElementById("reset-filters");
    if (resetBtn) resetBtn.addEventListener("click", () => {
        document.getElementById("search-name").value = "";
        document.getElementById("filter-category").value = "";
        document.getElementById("filter-status").value = "";
        document.getElementById("sort-price").value = "";
        applyFilters();
    });

    initOrderForm();
});

// ================== FETCH CATEGORÍAS ==================
function fetchCategorias() {
    fetch(`${API_URL}/Category`)
        .then(res => res.json())
        .then(data => {
            categorias = data;

            // Select de filtros
            const select = document.getElementById("filter-category");
            if (select) {
                categorias.forEach(cat => {
                    const option = document.createElement("option");
                    option.value = cat.id;
                    option.textContent = cat.name;
                    select.appendChild(option);
                });
            }

            // Carrusel de categorías
            const track = document.getElementById("categorias-track");
            if (track) {
                track.innerHTML = '';
                categorias.forEach(cat => {
                    const div = document.createElement('div');
                    div.className = 'category-card';
                    div.innerHTML = `
                        <h4>${cat.name}</h4>
                        <p>${cat.description || ''}</p>
                        <button class="category-btn" onclick="mostrarPlatos(${cat.id}, '${cat.name}')">Ver opciones</button>
                    `;
                    track.appendChild(div);
                });

                // Botones de scroll del carrusel
                const leftBtn = document.querySelector('.left-btn');
                const rightBtn = document.querySelector('.right-btn');
                if (leftBtn && rightBtn) {
                    leftBtn.addEventListener('click', () => {
                        track.scrollBy({ left: -300, behavior: 'smooth' });
                    });
                    rightBtn.addEventListener('click', () => {
                        track.scrollBy({ left: 300, behavior: 'smooth' });
                    });
                }
            }

        })
        .catch(err => console.error(err));
}

// ================== FETCH PLATOS ==================
function fetchPlatos() {
    fetch(`${API_URL}/Dish?onlyActive=false`)
        .then(res => res.json())
        .then(data => {
            platos = data || [];
            renderPlatos(platos);
        })
        .catch(err => {
            console.error(err);
            const container = document.getElementById("platos-container");
            if (container) container.innerHTML = "<p style='color:red'>Error cargando platos</p>";
        });
}

// ================== RENDER PLATOS ==================
function renderPlatos(lista) {
    const container = document.getElementById("platos-container");
    container.innerHTML = '';

    if (!lista.length) {
        container.innerHTML = "<p>No hay platos disponibles.</p>";
        return;
    }

    lista.forEach(plato => {
        const card = document.createElement("div");
        card.className = "plato-card";

        card.innerHTML = `
            <img src="${plato.image || 'placeholder.jpg'}" alt="${plato.name}">
            <h4>${plato.name}</h4>
            <p>${plato.description || ''}</p>
            <strong>$${plato.price?.toFixed(2) || ''}</strong>
            <button class="plato-btn" ${!plato.isActive ? 'disabled' : ''}>Agregar</button>
        `;

        const btn = card.querySelector(".plato-btn");
        btn.addEventListener("click", () => {
            addToCart({
                id: plato.id,
                name: plato.name,
                price: plato.price
            });
        });

        container.appendChild(card);
    });
}

// ================== FILTROS ==================
function applyFilters() {
    if (!platos.length) return;

    let filtered = [...platos];
    const nameFilter = document.getElementById("search-name")?.value.toLowerCase() || '';
    const categoryFilter = document.getElementById("filter-category")?.value || '';
    const statusFilter = document.getElementById("filter-status")?.value || '';
    const priceOrder = document.getElementById("sort-price")?.value || '';

    if (nameFilter) filtered = filtered.filter(p => p.name.toLowerCase().includes(nameFilter));
    if (categoryFilter) filtered = filtered.filter(p => p.category?.id == categoryFilter);
    if (statusFilter === "true") filtered = filtered.filter(p => p.isActive === true);
    else if (statusFilter === "false") filtered = filtered.filter(p => p.isActive === false);
    if (priceOrder) filtered.sort((a,b) => priceOrder === "asc" ? a.price - b.price : b.price - a.price);

    renderPlatos(filtered);
}
// ================== ESTILO PLATOS INACTIVOS ==================
// agregar CSS dinámico o en tu archivo CSS
const style = document.createElement('style');
style.innerHTML = `
    .plato-card.inactive {
        opacity: 0.5;
        pointer-events: none; /* bloquea click */
    }
`;
document.head.appendChild(style);

// ================== CARRITO ==================
function initCartSidebar() {
    const cartBtn = document.getElementById("cart-btn");
    const cartSidebar = document.getElementById("cart-sidebar");
    const closeCart = document.getElementById("close-cart");

    cartBtn.addEventListener("click", e => {
        e.preventDefault();
        cartSidebar.classList.toggle("open");
    });

    closeCart.addEventListener("click", () => cartSidebar.classList.remove("open"));

    document.addEventListener("click", e => {
        const isInsideSidebar = cartSidebar.contains(e.target);
        const isCartButton = cartBtn.contains(e.target);
        const isQtyButton = e.target.closest(".cart-item-qty");
        const isRemoveButton = e.target.classList.contains("cart-item-remove");

        if (cartSidebar.classList.contains("open") &&
            !isInsideSidebar &&
            !isCartButton &&
            !isQtyButton &&
            !isRemoveButton
        ) cartSidebar.classList.remove("open");
    });
}

function addToCart(plato) {
    const existing = carrito.find(item => item.id === plato.id);
    if (existing) existing.cantidad++;
    else carrito.push({...plato, cantidad:1});

    renderCartSidebar();
    saveCart();

    // Abrir sidebar automáticamente
    document.getElementById("cart-sidebar").classList.add("open");
}

function increaseQty(index) {
    carrito[index].cantidad++;
    renderCartSidebar();
    saveCart();
}

function decreaseQty(index) {
    if (carrito[index].cantidad > 1) carrito[index].cantidad--;
    else carrito.splice(index,1);
    renderCartSidebar();
    saveCart();
}

function removeFromCart(index) {
    carrito.splice(index,1);
    renderCartSidebar();
    saveCart();
}

function renderCartSidebar() {
    const container = document.getElementById("cart-items");
    const emptyBlock = document.getElementById("empty-cart");
    const orderSection = document.querySelector(".order-section");

    container.innerHTML = "";

    if (!carrito.length) {
        emptyBlock.style.display = "flex";
        orderSection.style.display = "none"; // oculta sección de pedido
        updateCartCount();
        return;
    }

    emptyBlock.style.display = "none";
    orderSection.style.display = "block"; // muestra sección de pedido

    let total = 0;

    carrito.forEach((item,index) => {
        total += item.price * item.cantidad;
        const div = document.createElement("div");
        div.className = "cart-item";
        div.innerHTML = `
            <div class="cart-item-info">
                <p class="cart-item-title">${item.name}</p>
                <p class="cart-item-price">$${item.price.toFixed(2)}</p>
                <div class="cart-item-qty">
                    <button onclick="decreaseQty(${index})">-</button>
                    <span>${item.cantidad}</span>
                    <button onclick="increaseQty(${index})">+</button>
                </div>
            </div>
            <button class="cart-item-remove" onclick="removeFromCart(${index})">🗑</button>
        `;
        container.appendChild(div);
    });

    const totalDiv = document.createElement("div");
    totalDiv.className = "carrito-total";
    totalDiv.innerHTML = `<strong>Total: $${total.toFixed(2)}</strong>`;
    totalDiv.style.marginTop = "10px";
    container.appendChild(totalDiv);

    updateCartCount();
}

function updateCartCount() {
    document.getElementById("cart-count").textContent = carrito.reduce((sum,item)=>sum+item.cantidad,0);
}

function saveCart() {
    localStorage.setItem("carrito",JSON.stringify(carrito));
}

function loadCart() {
    const stored = localStorage.getItem("carrito");
    if(stored) carrito = JSON.parse(stored);
    renderCartSidebar();
}

// ================== FORMULARIO PEDIDO ==================
function initOrderForm() {
    const orderType = document.getElementById("order-type");
    const addressGroup = document.getElementById("address-group");

    orderType.addEventListener("change", function() {
        addressGroup.style.display = this.value === "delivery" ? "block" : "none";
    });

    const orderForm = document.getElementById("order-form");
    orderForm.addEventListener("submit", async e => {
        e.preventDefault();

        const name = document.getElementById("name").value;
        const phone = document.getElementById("phone").value;
        const address = document.getElementById("address").value;

        const pedido = {
            orderType: orderType.value,
            customer: { name, phone, address: orderType.value === "delivery" ? address : null },
            items: carrito.map(item => ({
                dishId: item.id,
                name: item.name,
                quantity: item.cantidad,
                price: item.price
            }))
        };

        try {
            const res = await fetch(`${API_URL}/Order`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(pedido)
            });
            if(!res.ok) throw new Error("Error al enviar pedido");

            const data = await res.json();
            alert(`Pedido confirmado! Número: ${data.orderNumber}`);

            carrito = [];
            renderCartSidebar();
            saveCart();
            orderForm.reset();
        } catch(err) {
            console.error(err);
            alert("Hubo un problema al confirmar el pedido.");
        }
    });
}
