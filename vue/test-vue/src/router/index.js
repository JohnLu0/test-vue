import Vue from 'vue'
import Router from 'vue-router'
import HelloWorld from '../components/HelloWorld'
import Login from '../components/Login'
import Appindex from '../components/home/Appindex'
import Home from '../components/Home'
import Book from '../components/Book'
import Menus from '../components/Menus'
import SearchBar from '../components/SearchBar'
import EditForm from '../components/EditForm'
import Books from '../components/Books'
import SideMenu from '../components/SideMenu'
import NavMenu from '../components/common/NavMenu'
import Test from '../components/Test'
import LibraryIndex from '../components/LibraryIndex'

Vue.use(Router)

export default new Router({
  mode: 'history',
  routes: [
    {
      path: '/',
      name: 'HelloWorld',
      component: HelloWorld
    },
    {
      path: '/login',
      name: 'Login',
      component: Login
    },
    {
      path: '/index',
      name: 'Appindex',
      component: Appindex,
      meta: {
        requireAuth: true
      }
    },
    {
      path: '/home',
      name: 'Home',
      component: Home,
      // home页面并不需要被访问
      redirect: '/index',
      children: [
        {
          path: '/index',
          name: 'AppIndex',
          component: Appindex,
          meta: {
            requireAuth: true
          }
        }
      ]
    },
    {
      path: '/book',
      name: 'Book',
      component: Book
    },
    {
      path: '/menu',
      name: 'Menus',
      component: Menus
    },
    {
      path: '/searchBar',
      name: 'SearchBar',
      component: SearchBar
    },
    {
      path: '/editForm',
      name: 'EditForm',
      component: EditForm
    },
    {
      path: '/books',
      name: 'Books',
      component: Books
    },
    {
      path: '/sideMenu',
      name: 'SideMenu',
      component: SideMenu
    },
    {
      path: '/navMenu',
      name: 'NavMenu',
      component: NavMenu
    },
    {
      path: '/test',
      name: 'Test',
      component: Test
    },
    {
      path: '/libraryIndex',
      name: 'LibraryIndex',
      component: LibraryIndex
    }

  ]
})
