# Your-Quick-City 城镇地图插件使用文档

## 程序目录内容

  
### Internals

包内模块，描述了多数预先编写和不允许用户修改的功能。

  
> Interfaces

**ITask：**
任务接口，用于管理程序运行时执行的功能。

**IStepwiseTask：**
分步任务接口，用于管理程序运行时需要分步执行执行的功能。

  
> Behaviours

**MapBldgBaseDiagramGenerator：**
分析用户预定义的规则，印刷地图基底建筑物数据和执行基本随机运算。
程序接受用矩阵表示的地图并处理。

**MapBldgEntityGenerator：**
将印刷的矩阵地图实例化至引擎场景。

**MapBldgStructureDiagramGenerator：**
分析用户预定义的规则，在输入的地图矩阵上印刷子结构。

**MapStuffDataAnalyzer：**
分析用户定义的次级地图实体模板数据，转化为程序能够识别的数据结构。

**MapStuffDistributionDiagramGenerator：**
分析次级地图实体数据，生成用复合柱状图表示的地图密度-生成权重图。

**MapStuffEntityGenerator：**
将印刷的次级地图实体实例化至引擎场景。

**MapTerrainDetectorGenerator：**
在场景内实例化由统一接口控制的地图密度分析器，执行地图的密度分析。

**MapTileCoordsGenerator：**
执行地图的细分坐标生成，能根据用户预设的分辨率生成更细致的坐标。

  
> Properties

**MapNodeData：**
表示矩阵节点的数据结构。

  
> Utilties

工具类如异常/集合等若干。

  
### API

允许用户进行使用或部分覆盖实现的api/函数。
  

> Entities

**Map：** 用于表示地图的实体类。（接口+基本实现+ScriptableObject实现）

**Shape：** 用于表示地图形状轮廓的实体类。（接口+基本实现+ScriptableObject实现）

**Stuff：** 用于表示地图次级实体的实体类。（接口+基本实现+ScriptableObject实现）

**Structure：** 用于表示地图子结构的实体类。（接口+基本实现+ScriptableObject实现）

  
> Interfaces

**IDebugLogger：** 
表示实现类能够在控制台中输出调试信息。

**IMapObjParent：**
地图物体父级，表示实现类能够提供对地图实体的场景内层级结构管理功能。

**MapTerrainDetector：**
地图地形探测器。实现类通过生成特定的数据完成对地图的分析。

  
> Static

**Map：**
静态生成地图的API。在控制器内实例化该类以构造虚拟地图数据和执行操作。

  
### EngineImpl

引擎的实现层。部分对引擎有特殊实现要求的实体将会在此实现。

**UnityPhysicalTerrainDetector：**
基于Unity物理API的地图地形探测器。
