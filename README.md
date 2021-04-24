# 20210424
Detección de anomalías en llamadas telefónicas.
Este es un ejemplo en modo consola de una aplicación que detecta anomalías en las llamadas hechas en una oficina. La data viene de dos archivos planos que contiene la fecha y las horas acumulada de las llamadas realizadas en formato decimal. Para el estudio se utilizará Microsoft.ML con el algoritmo SRCNN (Microsoft.ML.TimeSeries), este paquete se instala por medio del instalador de packages NuGet, el NuGet se encuentra en el Visual Studio. 
Dentro de llamadas.cs se consigue:
public class Llamada // Definicion archivo de entradsa
public class PrediccionDeLlamada // Predicciones 
Dentro de Program.cs se consigue:
static void DetectAnomaly // Detecta Anomalia
static int DetectPeriod // Determina de período
Main // Lazo principal.

Los archivos:
llamadas-telefonicas.csv // llamadas sin anomalias
llamadas-telefonicas-anomalias.csv // llamadas con anomalias
deben ser llamados cambiando la variable static _dataPath desde la definición en el Program.cs
