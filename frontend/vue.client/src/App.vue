<template>
  <div id="app">
    <header class="app-header">
      <h1 class="title">Bank Software</h1>
      <nav class="main-nav">
        <RouterLink to="/" class="nav-link" active-class="active" exact>Loans list</RouterLink>
        <RouterLink to="/create" class="nav-link" active-class="active">Create loan</RouterLink>
      </nav>
    </header>
    
    <Toast />

    <main class="app-main">
      <RouterView v-slot="{ Component }">
        <transition name="fade" mode="out-in">
          <component :is="Component" />
        </transition>
      </RouterView>
    </main>

    <footer class="app-footer">
      <small>© Bank Software</small>
    </footer>
  </div>
</template>

<script>
import Toast from 'primevue/toast';
export default {
  name: 'App',
  components: {
    Toast
  },
}
</script>

<style scoped>
#app {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  font-family: system-ui, -apple-system, "Segoe UI", Roboto, "Helvetica Neue", Arial;
  color: #222;
}

.app-header {
  display: flex;
  align-items: center;
  justify-content: flex-start;
  padding: 16px 20px;
  background: #f5f7fb;
  border-bottom: 1px solid #e6e9ef;
  gap: 20px;
}

.title {
  margin: 0;
  font-size: 30px;
  font-weight: 600;
}

.main-nav {
  display: flex;
  gap: 12px;
  align-items: center;
}

.nav-link {
  text-decoration: none;
  color: #333;
  padding: 6px 10px;
  border-radius: 6px;
}

.nav-link.active {
  background: #1f6feb;
  color: #fff;
}

.app-main {
  flex: 1;
  padding: 20px;
}

.app-footer {
  padding: 12px 20px;
  background: #fafafa;
  border-top: 1px solid #eee;
  text-align: center;
  font-size: 13px;
  color: #666;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity .18s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
