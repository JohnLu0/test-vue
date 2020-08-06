<template>
  <el-form >
    <el-form-item class="list-todo list activeListClass" v-for="item in items" :key="item">
      <span class="icon-lock" v-if="item.locked">{{item.title}}</span>
      <input type="text" v-model="item.title" />
    </el-form-item>
     <!-- v-for 列表渲染-->
     <!-- v-if 条件渲染-->
  </el-form>
   <!-- <el-carousel indicator-position="outside">
      <el-carousel-item v-for="item in items" :key="item">
        <h3>{{ item.title }}</h3>
      </el-carousel-item>
    </el-carousel>-->
 <!-- <el-calendar v-model="value">
  </el-calendar>-->

</template>

<script>
export default {
  name: 'Menus',
  data () { // data函数
    return {
      items: [{ id: 1, title: '星期一', count: 1, locked: true }, // 菜单的模拟数据
        { id: 2, title: '星期二', count: 2, locked: true }, {
          id: 3, title: '星期三', count: 3, locked: false}]
    }
  }
/*  data() {
    return {
      value: new Date()
    }
  }*/
}
</script>

<style>
/*  .el-carousel__item h3 {
    color: #475669;
    font-size: 18px;
    opacity: 0.75;
    line-height: 300px;
    margin: 0;
  }

  .el-carousel__item:nth-child(2n) {
    background-color: #99a9bf;
  }

  .el-carousel__item:nth-child(2n+1) {
    background-color: #d3dce6;
  }*/
</style>
