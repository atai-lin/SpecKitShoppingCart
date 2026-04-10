// product-filter.js - AJAX and History API logic for product filtering

let filterState = {
    categories: [],
    materials: [],
    colors: [],
    q: '',
    sort: '',
    page: 1
};

function initFilterState() {
    const params = new URLSearchParams(window.location.search);
    filterState.categories = params.getAll('categories').map(id => parseInt(id));
    filterState.materials = params.getAll('materials');
    filterState.colors = params.getAll('colors');
    filterState.q = document.getElementById('qInput')?.value || '';
    filterState.sort = document.getElementById('sortByInput')?.value || 'newest';
    filterState.page = parseInt(params.get('pageIndex')) || 1;

    updateUISync();
}

function updateUISync() {
    document.querySelectorAll('.filter-checkbox').forEach(cb => {
        const type = cb.dataset.type;
        const val = cb.value;
        const isChecked = filterState[type].includes(type === 'categories' ? parseInt(val) : val);
        cb.checked = isChecked;
        
        // Update visual indicators
        const indicator = cb.parentElement.querySelector('.check-indicator');
        if (indicator) {
            indicator.style.opacity = isChecked ? '1' : '0';
        }
        
        const colorIndicator = cb.parentElement.querySelector('.color-indicator');
        if (colorIndicator) {
            if (isChecked) {
                colorIndicator.classList.add('ring-1', 'ring-offset-2', 'ring-atelier-dark');
            } else {
                colorIndicator.classList.remove('ring-1', 'ring-offset-2', 'ring-atelier-dark');
            }
        }
    });
}

function toggleFilter(type, value) {
    const val = type === 'categories' ? parseInt(value) : value;
    const index = filterState[type].indexOf(val);
    
    if (index > -1) {
        filterState[type].splice(index, 1);
    } else {
        filterState[type].push(val);
    }
    
    filterState.page = 1; // Reset to first page on filter change
    updateProductList();
}

async function updateProductList() {
    const overlay = document.getElementById('loadingOverlay');
    overlay?.classList.remove('hidden');

    const queryParams = new URLSearchParams();
    filterState.categories.forEach(id => queryParams.append('categories', id));
    filterState.materials.forEach(m => queryParams.append('materials', m));
    filterState.colors.forEach(c => queryParams.append('colors', c));
    if (filterState.q) queryParams.append('q', filterState.q);
    if (filterState.sort) queryParams.append('sort', filterState.sort);
    queryParams.append('page', filterState.page);

    try {
        const response = await fetch(`/api/product-list?${queryParams.toString()}`);
        if (!response.ok) throw new Error('Network response was not ok');
        
        const data = await response.json();
        renderProducts(data.products);
        renderFacets(data.facets);
        renderPagination(data.pagination);
        
        // Update URL silently
        const newUrl = `${window.location.pathname}?${queryParams.toString()}`;
        window.history.pushState(filterState, '', newUrl);
        
        updateUISync();
    } catch (error) {
        console.error('Error fetching products:', error);
    } finally {
        overlay?.classList.add('hidden');
    }
}

function renderProducts(products) {
    const container = document.getElementById('productGrid');
    if (!container) return;

    if (products.length === 0) {
        container.innerHTML = `
            <div class="bg-white border border-atelier-divider p-24 text-center">
                <p class="text-atelier-gray text-lg font-light italic font-work">No essentials found matching your selection.</p>
            </div>`;
        return;
    }

    let html = '<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-x-[32px] gap-y-[80px]">';
    products.forEach((product, i) => {
        const isOffsetDown = (i == 1 || i == 5 || (i > 5 && i % 6 == 1) || (i > 5 && i % 6 == 5));
        const isOffsetUp = (i == 3 || (i > 5 && i % 6 == 3));
        
        const imgSrc = product.imageUrl || '/img/placeholder.jpg';
        
        html += `
            <div class="flex flex-col ${isOffsetDown ? "lg:pt-12" : (isOffsetUp ? "lg:-mt-12" : "lg:pb-12")}">
                <div class="relative aspect-[3/4] bg-atelier-footer overflow-hidden group">
                    <img src="${imgSrc}" alt="${product.name}" class="w-full h-full object-cover transition-transform duration-1000 group-hover:scale-105">
                    <div class="absolute inset-0 bg-atelier-overlay backdrop-blur-[1px] opacity-0 group-hover:opacity-100 transition-opacity duration-500 flex items-center justify-center">
                        <a href="/Products/Detail?id=${product.id}" 
                           class="bg-white text-atelier-dark text-[12px] uppercase tracking-[1.2px] px-6 py-3 shadow-[0px_24px_48px_-12px_rgba(50,50,51,0.08)] hover:bg-atelier-dark hover:text-white transition-all transform translate-y-4 group-hover:translate-y-0 duration-700 font-work">
                            Quick View
                        </a>
                    </div>
                </div>
                <div class="mt-6 space-y-1">
                    <div class="flex justify-between items-start leading-none">
                        <h2 class="text-[18px] font-bold text-atelier-dark font-manrope tracking-atelier-tight leading-[28px]">${product.name}</h2>
                        <span class="text-[14px] leading-[20px] text-atelier-gray font-normal font-work">$${product.price.toLocaleString()}</span>
                    </div>
                    <p class="text-[12px] uppercase tracking-[0.6px] text-atelier-gray font-normal font-work leading-[16px]">${product.material}</p>
                </div>
            </div>`;
    });
    html += '</div>';
    container.innerHTML = html;
}

function renderFacets(facets) {
    for (const [name, count] of Object.entries(facets.categoryCounts)) {
        const el = document.querySelector(`[data-facet-category="${name}"]`);
        if (el) el.textContent = count;
    }
    for (const [name, count] of Object.entries(facets.materialCounts)) {
        const el = document.querySelector(`[data-facet-material="${name}"]`);
        if (el) el.textContent = count;
    }
}

function renderPagination(pagination) {
    // T017/T019 will refine this, but basic version for now
}

document.addEventListener('DOMContentLoaded', () => {
    initFilterState();

    document.querySelectorAll('.filter-checkbox').forEach(cb => {
        cb.addEventListener('change', (e) => {
            toggleFilter(e.target.dataset.type, e.target.value);
        });
    });

    window.addEventListener('popstate', (e) => {
        if (e.state) {
            filterState = e.state;
            updateUISync();
            updateProductList();
        } else {
            initFilterState();
            updateProductList();
        }
    });
});
