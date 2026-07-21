(() => {
  /* ——— Search ——— */
  const input = document.getElementById("search-input");
  const panel = document.getElementById("search-results");
  let index = null;
  let loading = null;
  let activeHit = -1;

  async function loadIndex() {
    if (index) return index;
    if (loading) return loading;
    loading = fetch("/search.json")
      .then((r) => r.json())
      .then((data) => {
        index = data;
        return data;
      })
      .finally(() => {
        loading = null;
      });
    return loading;
  }

  function escapeHtml(s) {
    return String(s)
      .replace(/&/g, "&amp;")
      .replace(/</g, "&lt;")
      .replace(/>/g, "&gt;")
      .replace(/"/g, "&quot;");
  }

  function currentHits() {
    return [...panel.querySelectorAll(".search-hit")];
  }

  function setActiveHit(i) {
    const hits = currentHits();
    hits.forEach((el) => el.classList.remove("active"));
    if (!hits.length) {
      activeHit = -1;
      return;
    }
    activeHit = ((i % hits.length) + hits.length) % hits.length;
    hits[activeHit].classList.add("active");
    hits[activeHit].scrollIntoView({ block: "nearest" });
  }

  function render(hits) {
    activeHit = -1;
    if (!hits.length) {
      panel.hidden = true;
      panel.innerHTML = "";
      return;
    }
    panel.hidden = false;
    panel.innerHTML = hits
      .slice(0, 10)
      .map(
        (h) => `
      <a class="search-hit" href="${h.url}" role="option">
        <strong>${escapeHtml(h.title)}</strong>
        <span>${escapeHtml(h.group)}</span>
        <p>${escapeHtml(h.snippet)}…</p>
      </a>`
      )
      .join("");
  }

  if (input && panel) {
    input.addEventListener("input", async () => {
      const q = input.value.trim().toLowerCase();
      if (q.length < 2) {
        render([]);
        return;
      }
      const data = await loadIndex();
      const hits = data.filter(
        (item) =>
          item.title.toLowerCase().includes(q) ||
          item.snippet.toLowerCase().includes(q) ||
          item.group.toLowerCase().includes(q)
      );
      render(hits);
    });

    input.addEventListener("keydown", (e) => {
      const hits = currentHits();
      if (e.key === "Escape") {
        input.value = "";
        render([]);
        input.blur();
        return;
      }
      if (!hits.length) return;
      if (e.key === "ArrowDown") {
        e.preventDefault();
        setActiveHit(activeHit + 1);
      } else if (e.key === "ArrowUp") {
        e.preventDefault();
        setActiveHit(activeHit - 1);
      } else if (e.key === "Enter" && activeHit >= 0) {
        e.preventDefault();
        hits[activeHit].click();
      }
    });

    document.addEventListener("click", (e) => {
      if (!panel.contains(e.target) && e.target !== input) {
        panel.hidden = true;
      }
    });
  }

  /* Focus search with / */
  document.addEventListener("keydown", (e) => {
    if (e.key !== "/" || !input) return;
    const tag = (e.target && e.target.tagName) || "";
    if (tag === "INPUT" || tag === "TEXTAREA" || e.target.isContentEditable) return;
    e.preventDefault();
    input.focus();
    input.select();
  });

  /* ——— Mobile sidebar ——— */
  const toggle = document.getElementById("nav-toggle");
  const backdrop = document.getElementById("sidebar-backdrop");
  const sidebar = document.getElementById("sidebar");

  function setSidebar(open) {
    document.body.classList.toggle("sidebar-open", open);
    if (toggle) toggle.setAttribute("aria-expanded", open ? "true" : "false");
    if (backdrop) backdrop.hidden = !open;
  }

  if (toggle && sidebar) {
    toggle.addEventListener("click", () => {
      setSidebar(!document.body.classList.contains("sidebar-open"));
    });
    if (backdrop) {
      backdrop.addEventListener("click", () => setSidebar(false));
      backdrop.hidden = false;
      backdrop.hidden = true;
    }
  } else if (toggle) {
    toggle.style.display = "none";
  }

  /* ——— Reading progress ——— */
  const bar = document.getElementById("reading-progress");
  const article = document.querySelector(".article");
  if (bar && article) {
    const onScroll = () => {
      const rect = article.getBoundingClientRect();
      const total = article.offsetHeight - window.innerHeight;
      const scrolled = Math.min(Math.max(-rect.top, 0), Math.max(total, 1));
      const pct = total > 0 ? (scrolled / total) * 100 : 0;
      bar.style.width = `${pct}%`;
    };
    window.addEventListener("scroll", onScroll, { passive: true });
    onScroll();
  }

  /* ——— Copy code blocks ——— */
  document.querySelectorAll(".prose pre").forEach((pre) => {
    const wrap = document.createElement("div");
    wrap.className = "code-block";
    pre.parentNode.insertBefore(wrap, pre);
    wrap.appendChild(pre);

    const btn = document.createElement("button");
    btn.type = "button";
    btn.className = "copy-btn";
    btn.textContent = "Copy";
    btn.addEventListener("click", async () => {
      const text = pre.innerText;
      try {
        await navigator.clipboard.writeText(text);
        btn.textContent = "Copied";
        setTimeout(() => {
          btn.textContent = "Copy";
        }, 1400);
      } catch {
        btn.textContent = "Failed";
        setTimeout(() => {
          btn.textContent = "Copy";
        }, 1400);
      }
    });
    wrap.appendChild(btn);
  });

  /* ——— On-this-page active heading ——— */
  const tocLinks = [...document.querySelectorAll(".on-this-page a")];
  if (tocLinks.length) {
    const map = tocLinks
      .map((a) => {
        const id = decodeURIComponent(a.getAttribute("href") || "").replace("#", "");
        const el = id ? document.getElementById(id) : null;
        return el ? { a, el } : null;
      })
      .filter(Boolean);

    const syncToc = () => {
      let current = map[0];
      for (const item of map) {
        if (item.el.getBoundingClientRect().top <= 120) current = item;
      }
      tocLinks.forEach((a) => a.classList.remove("active"));
      if (current) current.a.classList.add("active");
    };
    window.addEventListener("scroll", syncToc, { passive: true });
    syncToc();
  }

  /* Scroll active sidebar item into view */
  const activeNav = document.querySelector(".side-nav a.active");
  if (activeNav) {
    activeNav.scrollIntoView({ block: "nearest" });
  }
})();
